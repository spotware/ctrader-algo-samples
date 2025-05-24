using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace cAlgo.Robots
{
    public class PredictionResult
    {
        public SignalType Signal { get; set; }
        public double Confidence { get; set; }
    }
    
    public enum SignalType
    {
        None,
        Buy,
        Sell
    }
    
    public class MLModelInterface
    {
        private string _modelPath;
        private string _pythonPath = "python3"; // Default Python path
        private string _scriptPath;
        
        public MLModelInterface(string modelPath)
        {
            _modelPath = modelPath;
            
            // Create script path in the same directory as the model
            string directory = Path.GetDirectoryName(_modelPath);
            _scriptPath = Path.Combine(directory, "predict.py");
            
            // Update Python path to use our virtual environment
            _pythonPath = "/Users/amirsakr/Ctrader ML/venv/bin/python";
            
            // Ensure the Python script exists
            EnsurePythonScriptExists();
        }
        
        public PredictionResult Predict(double[] features)
        {
            try
            {
                // Convert features to JSON
                string featuresJson = JsonConvert.SerializeObject(features);
                
                // Call Python script for prediction
                string predictionJson = CallPythonScript("predict", featuresJson);
                
                // Parse prediction result
                var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(predictionJson);
                
                if (result != null && result.ContainsKey("signal") && result.ContainsKey("confidence"))
                {
                    string signalStr = result["signal"].ToString();
                    double confidence = Convert.ToDouble(result["confidence"]);
                    
                    SignalType signal = SignalType.None;
                    if (signalStr.Equals("buy", StringComparison.OrdinalIgnoreCase))
                        signal = SignalType.Buy;
                    else if (signalStr.Equals("sell", StringComparison.OrdinalIgnoreCase))
                        signal = SignalType.Sell;
                    
                    return new PredictionResult { Signal = signal, Confidence = confidence };
                }
                
                return new PredictionResult { Signal = SignalType.None, Confidence = 0 };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ML prediction: {ex.Message}");
                return new PredictionResult { Signal = SignalType.None, Confidence = 0 };
            }
        }
        
        public void Retrain(Bars bars, int lookback = 500)
        {
            try
            {
                // Prepare training data
                var trainingData = new Dictionary<string, object>();
                
                // Extract price data
                var closes = new List<double>();
                var opens = new List<double>();
                var highs = new List<double>();
                var lows = new List<double>();
                var volumes = new List<double>();
                var timestamps = new List<long>();
                
                int count = Math.Min(lookback, bars.Count);
                for (int i = 0; i < count; i++)
                {
                    closes.Add(bars.Close[i]);
                    opens.Add(bars.Open[i]);
                    highs.Add(bars.High[i]);
                    lows.Add(bars.Low[i]);
                    volumes.Add(bars.TickVolume[i]);
                    timestamps.Add(bars.OpenTime[i].Ticks);
                }
                
                trainingData["close"] = closes;
                trainingData["open"] = opens;
                trainingData["high"] = highs;
                trainingData["low"] = lows;
                trainingData["volume"] = volumes;
                trainingData["timestamp"] = timestamps;
                
                // Convert to JSON
                string trainingDataJson = JsonConvert.SerializeObject(trainingData);
                
                // Call Python script for retraining
                CallPythonScript("retrain", trainingDataJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ML retraining: {ex.Message}");
            }
        }
        
        private string CallPythonScript(string action, string dataJson)
        {
            try
            {
                // Create process to run Python script
                var process = new Process();
                process.StartInfo.FileName = _pythonPath;
                process.StartInfo.Arguments = $"\"{_scriptPath}\" {action} \"{_modelPath}\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                
                // Start the process
                process.Start();
                
                // Send data to the script
                process.StandardInput.WriteLine(dataJson);
                process.StandardInput.Close();
                
                // Read the output
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                
                // Wait for the process to exit
                process.WaitForExit();
                
                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine($"Python script error: {error}");
                }
                
                return output.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling Python script: {ex.Message}");
                return "{}";
            }
        }
        
        private void EnsurePythonScriptExists()
        {
            // Check if the Python script exists, if not, create it
            if (!File.Exists(_scriptPath))
            {
                string pythonScript = @"
import sys
import json
import pickle
import numpy as np
from sklearn.ensemble import RandomForestClassifier
from sklearn.preprocessing import StandardScaler
import pandas as pd
import os

def create_features(data):
    # Convert data to pandas DataFrame if it's not already
    if isinstance(data, list):
        df = pd.DataFrame(data)
    else:
        df = pd.DataFrame([data])
    
    # Calculate technical indicators
    # SMA
    df['sma5'] = df['close'].rolling(window=5).mean()
    df['sma20'] = df['close'].rolling(window=20).mean()
    
    # EMA
    df['ema5'] = df['close'].ewm(span=5, adjust=False).mean()
    df['ema20'] = df['close'].ewm(span=20, adjust=False).mean()
    
    # RSI
    delta = df['close'].diff()
    gain = (delta.where(delta > 0, 0)).rolling(window=14).mean()
    loss = (-delta.where(delta < 0, 0)).rolling(window=14).mean()
    rs = gain / loss
    df['rsi'] = 100 - (100 / (1 + rs))
    
    # MACD
    df['macd'] = df['ema12'] - df['ema26']
    df['macd_signal'] = df['macd'].ewm(span=9, adjust=False).mean()
    df['macd_hist'] = df['macd'] - df['macd_signal']
    
    # Bollinger Bands
    df['bb_middle'] = df['close'].rolling(window=20).mean()
    df['bb_std'] = df['close'].rolling(window=20).std()
    df['bb_upper'] = df['bb_middle'] + 2 * df['bb_std']
    df['bb_lower'] = df['bb_middle'] - 2 * df['bb_std']
    
    # Price change
    df['price_change'] = df['close'].pct_change()
    
    # Volatility
    df['volatility'] = df['close'].rolling(window=20).std()
    
    # Fill NaN values
    df = df.fillna(0)
    
    return df

def load_or_create_model(model_path):
    if os.path.exists(model_path):
        try:
            with open(model_path, 'rb') as f:
                model_data = pickle.load(f)
                return model_data['model'], model_data['scaler']
        except Exception as e:
            print(f""Error loading model: {e}"")
    
    # Create new model if loading fails or file doesn't exist
    model = RandomForestClassifier(n_estimators=100, random_state=42)
    scaler = StandardScaler()
    return model, scaler

def predict(model_path, features_json):
    # Parse features
    features = json.loads(features_json)
    
    # Load model
    model, scaler = load_or_create_model(model_path)
    
    # Create features
    df = create_features(features)
    
    # Select relevant features for prediction
    feature_cols = [col for col in df.columns if col not in ['timestamp']]
    X = df[feature_cols].values
    
    # Scale features
    X_scaled = scaler.transform(X)
    
    # Make prediction
    try:
        # Get probabilities
        proba = model.predict_proba(X_scaled)[0]
        
        # Get class with highest probability
        pred_class = model.classes_[np.argmax(proba)]
        confidence = np.max(proba)
        
        if pred_class == 1:
            signal = 'buy'
        elif pred_class == -1:
            signal = 'sell'
        else:
            signal = 'none'
            
        result = {
            'signal': signal,
            'confidence': float(confidence)
        }
    except Exception as e:
        print(f""Prediction error: {e}"")
        result = {
            'signal': 'none',
            'confidence': 0.0
        }
    
    return json.dumps(result)

def retrain(model_path, data_json):
    # Parse data
    data = json.loads(data_json)
    
    # Convert to DataFrame
    df = pd.DataFrame({
        'close': data['close'],
        'open': data['open'],
        'high': data['high'],
        'low': data['low'],
        'volume': data['volume'],
        'timestamp': data['timestamp']
    })
    
    # Create features
    df = create_features(df)
    
    # Create target variable (next day return)
    df['next_return'] = df['close'].shift(-1) / df['close'] - 1
    
    # Create classification target
    df['target'] = 0
    df.loc[df['next_return'] > 0.001, 'target'] = 1  # Buy signal
    df.loc[df['next_return'] < -0.001, 'target'] = -1  # Sell signal
    
    # Drop NaN values
    df = df.dropna()
    
    # Select features and target
    feature_cols = [col for col in df.columns if col not in ['timestamp', 'next_return', 'target']]
    X = df[feature_cols].values
    y = df['target'].values
    
    # Load or create model
    model, scaler = load_or_create_model(model_path)
    
    # Scale features
    X_scaled = scaler.fit_transform(X)
    
    # Train model
    try:
        model.fit(X_scaled, y)
        
        # Save model
        with open(model_path, 'wb') as f:
            pickle.dump({'model': model, 'scaler': scaler}, f)
        
        print(""Model trained and saved successfully"")
    except Exception as e:
        print(f""Training error: {e}"")

if __name__ == '__main__':
    if len(sys.argv) < 3:
        print(""Usage: python predict.py [predict|retrain] [model_path]"")
        sys.exit(1)
    
    action = sys.argv[1]
    model_path = sys.argv[2]
    
    # Read input data from stdin
    data_json = sys.stdin.readline().strip()
    
    if action == 'predict':
        result = predict(model_path, data_json)
        print(result)
    elif action == 'retrain':
        retrain(model_path, data_json)
    else:
        print(""Invalid action. Use 'predict' or 'retrain'"")
";
                File.WriteAllText(_scriptPath, pythonScript);
            }
        }
    }
}