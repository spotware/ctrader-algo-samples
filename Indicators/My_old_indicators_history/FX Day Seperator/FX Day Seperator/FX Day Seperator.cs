using System;
using System.Linq;
using System.Globalization;
using cAlgo.API;

//v1.01 Add Function: Day-Separator time setting;
//v1.02 Add Function: Setting Day-Separator Start-End as days range;
//v1.03 Add Function: Session Lither to highlight market sessions;
//v1.04 Add Function: User can set start and end time for each session;
//v1.05 Optimise    : Session background color up to Y-axis ±9999;
//v1.06 Optimise    : CodeStructure, User can set each session start and end;
//v1.07 Add Function: User can set color for each sessions;
//v1.08 Add Function: User can set Session Lighter On/Off in setting;
//v1.09 Add Function: User can set 5Min 1, 4Hour Gridlines On/Off; Month Lighter On/Off;
//      Optimise    : Framework .NET 6.0 ready;
//v1.10 Optimise    : Session lighter split to 5 groups (Assia, European, New York, London Fix, New York Close);
//                  : Each session has 2 background lighters, and extend to all indicator areas;
//      Add Function: Time of Daily Routine display as background text;
//      Add Function: MonthLighter covers 2 Years (current and NEXT year);
//v1.11 Optimise    : MonthLighter covers 3 Years (Past, Current and NEXT year);

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class FXDaySeperator : Indicator
    { 
        [Parameter("Day-Start Time:",   DefaultValue = "22:00:00",          Group = "Day Separator (Ver. 1.11)" )]  public string   DayStartTime    { get; set; }
        [Parameter("Days-Start From: ", DefaultValue = 365, MinValue = 0,   Group = "Day Separator (Ver. 1.11)" )]  public int      DaysStart       { get; set; }
        [Parameter("Days-End Till:",    DefaultValue = 7,   MinValue = 0,   Group = "Day Separator (Ver. 1.11)" )]  public int      DaysEnd         { get; set; }
        [Parameter("Hex Color Code",    DefaultValue = "#15FFFFFF",         Group = "Day Separator (Ver. 1.11)" )]  public string   DaySepColor     { get; set; }
        
        [Parameter("Min,Hour Gridlines",DefaultValue = true,                Group = "Min,Hour,Month Seperators" )]  public bool     MinHorLne       { get; set; }
        [Parameter("Month Lighter",     DefaultValue = false,               Group = "Min,Hour,Month Seperators" )]  public bool     MnthLit         { get; set; }
        
        [Parameter("Active",                DefaultValue = true,                                        Group = "Session Ligther")]  public bool LiterOn        { get; set; }
        [Parameter("Lighten  Past   Days",  DefaultValue = 5, MaxValue = 30, MinValue = 0, Step = 1,    Group = "Session Ligther")]  public int  PastDaysNumber { get; set; }
        [Parameter("Lighten Future Days",   DefaultValue = 1, MaxValue =  7, MinValue = 0, Step = 1,    Group = "Session Ligther")]  public int  FutrDaysNumber { get; set; }
        [Parameter("Longest TimeFrame",     DefaultValue = "Minute5",                                   Group = "Session Ligther")]  public TimeFrame LgstTmFrm { get; set; }

        [Parameter("Early Asia Start",      DefaultValue = "23:00:00",      Group = "Asia Session"    )]  public string   AS1S    { get; set; }
        [Parameter("Early Asia End",        DefaultValue = "23:59:59",      Group = "Asia Session"    )]  public string   AS1E    { get; set; }
        [Parameter("Early Asia Color",      DefaultValue = "#10FF0000",     Group = "Asia Session"    )]  public string   AS1CL   { get; set; }   //DarkRed Asia
        [Parameter("Major Asia Start",      DefaultValue = "00:00:00",      Group = "Asia Session"    )]  public string   AS2S    { get; set; }
        [Parameter("Major Asia End",        DefaultValue = "04:30:00",      Group = "Asia Session"    )]  public string   AS2E    { get; set; }
        [Parameter("Major Asia Color",      DefaultValue = "#10FF0000",     Group = "Asia Session"    )]  public string   AS2CL   { get; set; }   //DarkRed Asia

        [Parameter("Early Europe Start",    DefaultValue = "04:30:00",      Group = "European Session")]  public string   EZ1S    { get; set; }
        [Parameter("Early Europe End",      DefaultValue = "07:00:00",      Group = "European Session")]  public string   EZ1E    { get; set; }
        [Parameter("Early Europe Color",    DefaultValue = "#1000FF00",     Group = "European Session")]  public string   EZ1CL   { get; set; }   //DarkGrn Early Europe
        [Parameter("Morning Europe Start",  DefaultValue = "07:00:00",      Group = "European Session")]  public string   EZ2S    { get; set; }
        [Parameter("Morning Europe End",    DefaultValue = "11:30:00",      Group = "European Session")]  public string   EZ2E    { get; set; }
        [Parameter("Morning Europe Color",  DefaultValue = "#10FFFF00",     Group = "European Session")]  public string   EZ2CL   { get; set; }   //DarkYlw Morning Europe

        [Parameter("Early NewYork Start",   DefaultValue = "11:30:00",      Group = "New York Session")]  public string   NYES    { get; set; }
        [Parameter("Early NewYork End",     DefaultValue = "13:00:00",      Group = "New York Session")]  public string   NYEE    { get; set; }
        [Parameter("Early NewYork Color",   DefaultValue = "#1000FF00",     Group = "New York Session")]  public string   NYECL   { get; set; }   //DarkGrn Early NewYork
        [Parameter("News Trading Start",    DefaultValue = "13:00:00",      Group = "New York Session")]  public string   NWTS    { get; set; }
        [Parameter("News Trading End",      DefaultValue = "15:00:00",      Group = "New York Session")]  public string   NWTE    { get; set; }
        [Parameter("News Trading Color",    DefaultValue = "#10EEFF88",     Group = "New York Session")]  public string   NWTCL   { get; set; }   //LightGrey NeWsTrading

        [Parameter("London Fix Start",      DefaultValue = "15:00:00",      Group = "London Fix, NYBK")]  public string   LDFS    { get; set; }
        [Parameter("London Fix End",        DefaultValue = "17:00:00",      Group = "London Fix, NYBK")]  public string   LDFE    { get; set; }
        [Parameter("London Fix Color",      DefaultValue = "#10AA00FF",     Group = "London Fix, NYBK")]  public string   LDFCL   { get; set; }   //Purple London Fix
        [Parameter("N.Y. Break Start",      DefaultValue = "17:00:00",      Group = "London Fix, NYBK")]  public string   NYKS    { get; set; }
        [Parameter("N.Y. Break End",        DefaultValue = "19:00:00",      Group = "London Fix, NYBK")]  public string   NYKE    { get; set; }
        [Parameter("N.Y. Break Color",      DefaultValue = "#1000FF00",     Group = "London Fix, NYBK")]  public string   NYKCL   { get; set; }   //DarkGrn NewYork Break

        [Parameter("NewYork Close Start",   DefaultValue = "19:00:00",      Group = "New York, Close" )]  public string   NYCS    { get; set; }
        [Parameter("NewYork Close End",     DefaultValue = "21:00:00",      Group = "New York, Close" )]  public string   NYCE    { get; set; }
        [Parameter("NewYork Close Color",   DefaultValue = "#0800FFFF",     Group = "New York, Close" )]  public string   NYCCL   { get; set; }   //DarkTurquoise NewYork Close
        [Parameter("Day Ending Start",      DefaultValue = "21:00:00",      Group = "New York, Close" )]  public string   ENDS    { get; set; }
        [Parameter("Day Ending End",        DefaultValue = "22:00:00",      Group = "New York, Close" )]  public string   ENDE    { get; set; }
        [Parameter("Day Ending Color",      DefaultValue = "#050000FF",     Group = "New York, Close" )]  public string   ENDCL   { get; set; }   //DarkBlue Day Ending

        [Parameter("Active",                DefaultValue = true,            Group = "Daily Routine Marker"  )]  public bool     DailyRtMkr  { get; set; }
        [Parameter("New Day Start",         DefaultValue = "23:00:00",      Group = "Every Working Day"     )]  public string   RNWD        { get; set; }
        [Parameter("Tokyo Fix",             DefaultValue = "00:55:00",      Group = "Every Working Day"     )]  public string   RJPX        { get; set; }
        [Parameter("China Fix",             DefaultValue = "01:15:00",      Group = "Every Working Day"     )]  public string   RCNX        { get; set; }
        [Parameter("China Stock Open",      DefaultValue = "01:30:00",      Group = "Every Working Day"     )]  public string   RCNS        { get; set; }
        [Parameter("London Open",           DefaultValue = "07:00:00",      Group = "Every Working Day"     )]  public string   RLDO        { get; set; }
        [Parameter("Asia Stock Close",      DefaultValue = "07:00:00",      Group = "Every Working Day"     )]  public string   RASC        { get; set; }
        [Parameter("European Stock Open",   DefaultValue = "08:00:00",      Group = "Every Working Day"     )]  public string   RESO        { get; set; }
        [Parameter("New York Open",         DefaultValue = "12:00:00",      Group = "Every Working Day"     )]  public string   RNYO        { get; set; }
        [Parameter("US News",               DefaultValue = "13:30:00",      Group = "Every Working Day"     )]  public string   RNYN        { get; set; }
        [Parameter("US NYMEX Open",         DefaultValue = "14:00:00",      Group = "Every Working Day"     )]  public string   RNXO        { get; set; }
        [Parameter("US NYSE(Stock) Open",   DefaultValue = "14:30:00",      Group = "Every Working Day"     )]  public string   RNYS        { get; set; }
        [Parameter("US Data, Option Expiry",DefaultValue = "15:00:00",      Group = "Every Working Day"     )]  public string   RNYD        { get; set; }
        [Parameter("WME Benchmark Fix",     DefaultValue = "16:00:00",      Group = "Every Working Day"     )]  public string   RWMR        { get; set; }
        [Parameter("European Stock Close",  DefaultValue = "16:30:00",      Group = "Every Working Day"     )]  public string   RESC        { get; set; }
        [Parameter("London HomeTime",       DefaultValue = "17:00:00",      Group = "Every Working Day"     )]  public string   RLDC        { get; set; }
        [Parameter("Bank of Canada Fix",    DefaultValue = "17:00:00",      Group = "Every Working Day"     )]  public string   RCAX        { get; set; }
        [Parameter("New York Afternoon",    DefaultValue = "18:00:00",      Group = "Every Working Day"     )]  public string   RNYA        { get; set; }
        [Parameter("NZD Date Change",       DefaultValue = "18:00:00",      Group = "Every Working Day"     )]  public string   RNZR        { get; set; }
        [Parameter("NYMEX Close",           DefaultValue = "19:30:00",      Group = "Every Working Day"     )]  public string   RNXC        { get; set; }
        [Parameter("CME Close",             DefaultValue = "20:00:00",      Group = "Every Working Day"     )]  public string   RCME        { get; set; }
        [Parameter("US NYSE(Stock) Close",  DefaultValue = "21:00:00",      Group = "Every Working Day"     )]  public string   RNYC        { get; set; }
        [Parameter("API Crude Oil Stock",   DefaultValue = "21:30:00",      Group = "Every Tuesday"         )]  public string   RAPI        { get; set; }
        [Parameter("EIA Crude Oil Stock",   DefaultValue = "15:30:00",      Group = "Every Wednesday"       )]  public string   REIA        { get; set; }


        private Bars Seires_D1;     //Bars to load TF.Daily
        private DateTime LstStart;  //Time of LastTradingDayStart
        private DateTime DayStart;  //Time of DayStart
        private DateTime DayEnd;    //Time of DayEnd

        private readonly string pfx_DySp = "DaySep_",   fmt_DySp = "yyyy-MM-dd ";   //DaySeparator  : NamePrefix, ObjectName format;
        private readonly string pfx_AS1 = "AS1_",       pfx_AS2 = "AS2_";           //NamePrefix: Session Lighter Box;
        private readonly string pfx_EZ1 = "EZ1_",       pfx_EZ2 = "EZ2_";
        private readonly string pfx_NYE = "NYE_",       pfx_NWT = "NWT_";
        private readonly string pfx_LDF = "LDF_",       pfx_NYK = "NYK_";
        private readonly string pfx_NYC = "NYC_",       pfx_END = "END_";
        private readonly Color cl_Rtn = Color.FromHex("#55777777");                 //Color of daily routin marker
        private readonly string pfx_RNWD="RNWD_",       mkr_RNWD= "New\n   ⬩\nDay"; //NamePrefix, MarkerContent: DailyRoutine's Text;
        private readonly string pfx_RJPX="RJPX_",       mkr_RJPX= "TKY\n   ⬩\n Fix";
        private readonly string pfx_RCNX="RCNX_",       mkr_RCNX= "CNH\n   ⬩\n Fix";
        private readonly string pfx_RCNS="RCNS_",       mkr_RCNS= "STK\n   ⬩\n Opn";
        private readonly string pfx_RLDO="RLDO_",       mkr_RLDO= "LDN\nOpn\n   ˖\n\n ";
        private readonly string pfx_RASC="RASC_",       mkr_RASC= " \n\n　  ⬩\n AZ STK\n    Cls";
        private readonly string pfx_RESO="RESO_",       mkr_RESO= "EZ STK\n   Opn\n　  ˖\n\n ";
        private readonly string pfx_RNYO="RNYO_",       mkr_RNYO= "NYK\n   ⬩\nOpn";
        private readonly string pfx_RNXO="RNXO_",       mkr_RNXO= "NYMEX\n　  ⬩\n   Opn";
        private readonly string pfx_RNYN="RNYN_",       mkr_RNYN= "  US\n　⬩\nNews";
        private readonly string pfx_RNYS="RNYS_",       mkr_RNYS= "NYSE\n    ⬩\n Opn";
        private readonly string pfx_RNYD="RNYD_",       mkr_RNYD= "  US\n Data \n　⬩\n  Opt.\n  Exp.";
        private readonly string pfx_RWMR="RWMR_",       mkr_RWMR= "WMR\n　⬩\n  Fix";
        private readonly string pfx_RESC="RESC_",       mkr_RESC= " EZ \nSTK\n   ˖\n Cls\n ";
        private readonly string pfx_RLDC="RLDC_",       mkr_RLDC= " LDN\n Hme  \n　˖\n\n ";
        private readonly string pfx_RCAX="RCAX_",       mkr_RCAX= " \n\n   ⬩\n BoC  \n  Fix";
        private readonly string pfx_RNYA="RNYA_",       mkr_RNYA= " NY\n1pm\n   ˖\n\n ";
        private readonly string pfx_RNZR="RNZR_",       mkr_RNZR= " \n\n   ⬩\nNZD\n Roll";
        private readonly string pfx_RNXC="RNXC_",       mkr_RNXC= "NYMEX\n　  ⬩\n    Cls";
        private readonly string pfx_RCME="RCME_",       mkr_RCME= "CME\n   ⬩\n Cls";
        private readonly string pfx_RNYC="RNYC_",       mkr_RNYC= "NYSE\n    ⬩\n  Cls";
        private readonly string pfx_RAPI="RAPI_",       mkr_RAPI= "API\n  ⬩\n Oil";
        private readonly string pfx_REIA="REIA_",       mkr_REIA= "EIA\n  ⬩\n Oil";


        protected override void Initialize()
        {
            //Get TF.Day1 Bars
            Seires_D1 = MarketData.GetBars(TimeFrame.Daily);
            LstStart = Seires_D1.Last(1).OpenTime;
            DayStart = Seires_D1.LastBar.OpenTime;
            DayEnd = DayStart.AddDays(1);

            //Plot DaySeparator (-365 ~ +7 Days) of all TimeFrame      
            PlotDaySeprt();

            //Plot WokingHour on TradingDays
            PlotDayLight(); 
            
            //Plot MonthLighter on HalfYearChart
            PlotMthLight();

            //Plot Intraday VerticalLines
            DrawIntradayVertLnes();
            Bars.BarOpened += Bars_BarOpened;

            //Chart.DrawStaticText("Debug", TFGroup(TimeFrame), VerticalAlignment.Bottom, HorizontalAlignment.Center, Color.FromHex("AAEEDDCC")); 
        }

        public override void Calculate(int index) { }
        
        //ChartControl  - NewBar to Plot Intraday VerticalLines
        private void Bars_BarOpened(BarOpenedEventArgs obj)
        { 
            if (Seires_D1.LastBar.OpenTime > DayStart)
            {
                LstStart = Seires_D1.Last(1).OpenTime;
                DayStart = Seires_D1.LastBar.OpenTime;
                DayEnd = DayStart.AddDays(1);
                DrawIntradayVertLnes();
            }

        }

        //Plot DaySeparator 
        private void PlotDaySeprt() //On Past365 + Next7 EveryDay(include weekend), of all TimeFrame;
        {
            Color cl_DaySep = Color.FromHex(DaySepColor);                                                                                           //Convert Color code to Color
            DateTime dt_TdySepHr = DateTime.Parse(DateTime.Today.ToString(fmt_DySp) + DayStartTime).Add(-Application.UserTimeOffset);               //Get SplitTime Today, and Convert UserTime to ServerTime;
            for (DateTime crntDay = (dt_TdySepHr.AddDays(-DaysStart)); crntDay <= (dt_TdySepHr.AddDays(+DaysEnd)); crntDay = crntDay.AddDays(1))    //Separator Past365 ~ Next7 Days
            { Chart.DrawVerticalLine(pfx_DySp+crntDay.ToString(fmt_DySp), crntDay, cl_DaySep, 1, LineStyle.Solid); }                                //Plot VerticalLines

        }

        //Plot SessionLighter on TradingDays
        private void PlotDayLight() 
        {
            //Plot only in TimeFrame shorter than Setting 
            if ( !LiterOn || TimeFrame > LgstTmFrm ) return;

            //Define Parameters for and WorkHour Highlight: DateTime of FirstDate, LastDate;  
            DateTime startDate = (Bars.LastBar.OpenTime.AddDays(-PastDaysNumber)).Date; DayOfWeek FDoW = startDate.DayOfWeek;
            startDate = (FDoW==DayOfWeek.Saturday) ? startDate.AddDays(-1) : (FDoW==DayOfWeek.Sunday) ? startDate.AddDays(-2) : startDate; //Move startDate Friday, if allocated in Weekend
            DateTime last_Date = (Bars.LastBar.OpenTime.AddDays(+FutrDaysNumber)).Date; DayOfWeek LDoW = last_Date.DayOfWeek;
            last_Date = (LDoW==DayOfWeek.Saturday) ? last_Date.AddDays(+2) : (FDoW==DayOfWeek.Sunday) ? last_Date.AddDays(+1) : last_Date; //Move last_Date Monday, if allocated in Weekend

            //Define color for each working period
            Color cl_AS1 = Color.FromHex(AS1CL), cl_AS2 = Color.FromHex(AS2CL);     //DarkRed  Asia
            Color cl_EZ1 = Color.FromHex(EZ1CL), cl_EZ2 = Color.FromHex(EZ2CL);     //DarkBlue Europe
            Color cl_NYE = Color.FromHex(NYECL), cl_NWT = Color.FromHex(NWTCL);     //DarkYlw  NewYork
            Color cl_LDF = Color.FromHex(LDFCL), cl_NYK = Color.FromHex(NYKCL);     //Purple   London Fix
            Color cl_NYC = Color.FromHex(NYCCL), cl_END = Color.FromHex(ENDCL);     //DarkTurquoise NewYork Close

            //Prefix of each working period
            //Get startDate in string format
            string s_fstDay = startDate.ToString(fmt_DySp);     

            //Fix FirstDayTime for X1, X2 positions of all Periods                                                                                                                                   Initialize X1, X2 Positions of all Periods - Vars. to Plot
            DateTime dt_AS1S = DateTime.Parse(s_fstDay + AS1S).Add(-Application.UserTimeOffset).AddDays(-1), dt_AS1E = DateTime.Parse(s_fstDay + AS1E).Add(-Application.UserTimeOffset).AddDays(-1), dt_as1s = dt_AS1S, dt_as1e = dt_AS1E;  //EarlyAsia     StartTime, EndTime
            DateTime dt_AS2S = DateTime.Parse(s_fstDay + AS2S).Add(-Application.UserTimeOffset)            , dt_AS2E = DateTime.Parse(s_fstDay + AS2E).Add(-Application.UserTimeOffset)            , dt_as2s = dt_AS2S, dt_as2e = dt_AS2E;  //MajorAsia     StartTime, EndTime
            DateTime dt_EZ1S = DateTime.Parse(s_fstDay + EZ1S).Add(-Application.UserTimeOffset)            , dt_EZ1E = DateTime.Parse(s_fstDay + EZ1E).Add(-Application.UserTimeOffset)            , dt_ez1s = dt_EZ1S, dt_ez1e = dt_EZ1E;  //EarlyEurope   StartTime, EndTime
            DateTime dt_EZ2S = DateTime.Parse(s_fstDay + EZ2S).Add(-Application.UserTimeOffset)            , dt_EZ2E = DateTime.Parse(s_fstDay + EZ2E).Add(-Application.UserTimeOffset)            , dt_ez2s = dt_EZ2S, dt_ez2e = dt_EZ2E;  //MorningEurope StartTime, EndTime
            DateTime dt_NYES = DateTime.Parse(s_fstDay + NYES).Add(-Application.UserTimeOffset)            , dt_NYEE = DateTime.Parse(s_fstDay + NYEE).Add(-Application.UserTimeOffset)            , dt_nyes = dt_NYES, dt_nyee = dt_NYEE;  //Early NewYork StartTime, EndTime
            DateTime dt_NWTS = DateTime.Parse(s_fstDay + NWTS).Add(-Application.UserTimeOffset)            , dt_NWTE = DateTime.Parse(s_fstDay + NWTE).Add(-Application.UserTimeOffset)            , dt_nwts = dt_NWTS, dt_nwte = dt_NWTE;  //NewsTrading   StartTime, EndTime
            DateTime dt_LDFS = DateTime.Parse(s_fstDay + LDFS).Add(-Application.UserTimeOffset)            , dt_LDFE = DateTime.Parse(s_fstDay + LDFE).Add(-Application.UserTimeOffset)            , dt_ldfs = dt_LDFS, dt_ldfe = dt_LDFE;  //London Fix    StartTime, EndTime
            DateTime dt_NYKS = DateTime.Parse(s_fstDay + NYKS).Add(-Application.UserTimeOffset)            , dt_NYKE = DateTime.Parse(s_fstDay + NYKE).Add(-Application.UserTimeOffset)            , dt_nyks = dt_NYKS, dt_nyke = dt_NYKE;  //NewYork Break StartTime, EndTime
            DateTime dt_NYCS = DateTime.Parse(s_fstDay + NYCS).Add(-Application.UserTimeOffset)            , dt_NYCE = DateTime.Parse(s_fstDay + NYCE).Add(-Application.UserTimeOffset)            , dt_nycs = dt_NYCS, dt_nyce = dt_NYCE;  //NewYork Close StartTime, EndTime
            DateTime dt_ENDS = DateTime.Parse(s_fstDay + ENDS).Add(-Application.UserTimeOffset)            , dt_ENDE = DateTime.Parse(s_fstDay + ENDE).Add(-Application.UserTimeOffset)            , dt_ends = dt_ENDS, dt_ende = dt_ENDE;  //DayEnding     StartTime, EndTime
            //...              for X-Positions of DailyRoutineMarkers                                   Ini. DailyRoutine's X-Positions
            DateTime dt_RNWD = DateTime.Parse(s_fstDay + RNWD).Add(-Application.UserTimeOffset).AddDays(-1), dt_rnwd = dt_RNWD;         //23:00 New Day Start
            DateTime dt_RJPX = DateTime.Parse(s_fstDay + RJPX).Add(-Application.UserTimeOffset)            , dt_rjpx = dt_RJPX;         //00:55 Tokyo Fix           (Tokyo    8:55am)
            DateTime dt_RCNX = DateTime.Parse(s_fstDay + RCNX).Add(-Application.UserTimeOffset)            , dt_rcnx = dt_RCNX;         //01:15 China Fix           (Beijing  9:15am)
            DateTime dt_RCNS = DateTime.Parse(s_fstDay + RCNS).Add(-Application.UserTimeOffset)            , dt_rcns = dt_RCNS;         //01:30 China Stock Open    (Beijing  9:30am)
            DateTime dt_RLDO = DateTime.Parse(s_fstDay + RLDO).Add(-Application.UserTimeOffset)            , dt_rldo = dt_RLDO;         //07:00 London Open
            DateTime dt_RASC = DateTime.Parse(s_fstDay + RASC).Add(-Application.UserTimeOffset)            , dt_rasc = dt_RASC;         //07:00 Asia Stock Close    (Beijing 15:00pm)
            DateTime dt_RESO = DateTime.Parse(s_fstDay + RESO).Add(-Application.UserTimeOffset)            , dt_reso = dt_RESO;         //08:00 EuroZone Stock Open
            DateTime dt_RNYO = DateTime.Parse(s_fstDay + RNYO).Add(-Application.UserTimeOffset)            , dt_rnyo = dt_RNYO;         //12:00 New York Open       (NewYork  7:00am)
            DateTime dt_RNYN = DateTime.Parse(s_fstDay + RNYN).Add(-Application.UserTimeOffset)            , dt_rnyn = dt_RNYN;         //13:30 US News             (NewYork  8:30am)
            DateTime dt_RNXO = DateTime.Parse(s_fstDay + RNXO).Add(-Application.UserTimeOffset)            , dt_rnxo = dt_RNXO;         //14:00 US NYMEX Open       (NewYork  9:00am)
            DateTime dt_RNYS = DateTime.Parse(s_fstDay + RNYS).Add(-Application.UserTimeOffset)            , dt_rnys = dt_RNYS;         //14:30 US NYSE(Stock) Open (NewYork  9:30am)
            DateTime dt_RNYD = DateTime.Parse(s_fstDay + RNYD).Add(-Application.UserTimeOffset)            , dt_rnyd = dt_RNYD;         //15:00 US Data/OptionExpiry(NewYork 10:00am)
            DateTime dt_RWMR = DateTime.Parse(s_fstDay + RWMR).Add(-Application.UserTimeOffset)            , dt_rwmr = dt_RWMR;         //16:00 WMR Benchmark Fix   (London  16:00pm)
            DateTime dt_RESC = DateTime.Parse(s_fstDay + RESC).Add(-Application.UserTimeOffset)            , dt_resc = dt_RESC;         //16:30 European Stock Close(London  16:30pm)
            DateTime dt_RLDC = DateTime.Parse(s_fstDay + RLDC).Add(-Application.UserTimeOffset)            , dt_rldc = dt_RLDC;         //17:00 London HomeTime     (London  17:00pm)
            DateTime dt_RCAX = DateTime.Parse(s_fstDay + RCAX).Add(-Application.UserTimeOffset)            , dt_rcax = dt_RCAX;         //17:00 Bank of Canada Fix  (London  17:00pm)
            DateTime dt_RNYA = DateTime.Parse(s_fstDay + RNYA).Add(-Application.UserTimeOffset)            , dt_rnya = dt_RNYA;         //18:00 New York Afternoon  (NewYork 13:00pm)
            DateTime dt_RNZR = DateTime.Parse(s_fstDay + RNZR).Add(-Application.UserTimeOffset)            , dt_rnzr = dt_RNZR;         //18:00 NZD Date Change     (WLG      7:00am)
            DateTime dt_RNXC = DateTime.Parse(s_fstDay + RNXC).Add(-Application.UserTimeOffset)            , dt_rnxc = dt_RNXC;         //19:30 US NYMEX Close      (NewYork 14:30pm)
            DateTime dt_RCME = DateTime.Parse(s_fstDay + RCME).Add(-Application.UserTimeOffset)            , dt_ecme = dt_RCME;         //20:00 US CME Close        (NewYork 15:00pm)
            DateTime dt_RNYC = DateTime.Parse(s_fstDay + RNYC).Add(-Application.UserTimeOffset)            , dt_rnyc = dt_RNYC;         //21:00 US NYSE(Stock) Close(NewYork 16:00pm)
            DateTime dt_RAPI = DateTime.Parse(s_fstDay + RAPI).Add(-Application.UserTimeOffset)            , dt_rapi = dt_RAPI;         //21:30 US API Oil Stock CHG(NewYork 21:30pm Tuesday Only)
            DateTime dt_REIA = DateTime.Parse(s_fstDay + REIA).Add(-Application.UserTimeOffset)            , dt_reia = dt_REIA;         //15:30 US EIA Oil Stock CHG(NewYork 10:30pm Wednesday Only)

            //Initialize Accumulator, Variable of rectangle
            int i = 0; ChartRectangle rt_DLit; ChartText tx_RMkr;

            //Day by Day plot Retangles
            for ( DateTime crntme = startDate; crntme <= last_Date; crntme = startDate.AddDays(i) ) 
            {   
                if ( crntme.DayOfWeek != DayOfWeek.Saturday && crntme.DayOfWeek != DayOfWeek.Sunday ) //Skip Weekend
                {
                    //Reset Ploting X1 X2 DateTime
                    dt_as1s = dt_AS1S.AddDays(i); dt_as1e = dt_AS1E.AddDays(i); if (dt_as1s.DayOfWeek == DayOfWeek.Sunday) {dt_as1s = dt_as1s.AddDays(-2);}  //Move Start from Sunday evening to Friday evening;
                    dt_as2s = dt_AS2S.AddDays(i); dt_as2e = dt_AS2E.AddDays(i);
                    dt_ez1s = dt_EZ1S.AddDays(i); dt_ez1e = dt_EZ1E.AddDays(i);
                    dt_ez2s = dt_EZ2S.AddDays(i); dt_ez2e = dt_EZ2E.AddDays(i);
                    dt_nyes = dt_NYES.AddDays(i); dt_nyee = dt_NYEE.AddDays(i);
                    dt_nwts = dt_NWTS.AddDays(i); dt_nwte = dt_NWTE.AddDays(i);
                    dt_ldfs = dt_LDFS.AddDays(i); dt_ldfe = dt_LDFE.AddDays(i);
                    dt_nyks = dt_NYKS.AddDays(i); dt_nyke = dt_NYKE.AddDays(i);
                    dt_nycs = dt_NYCS.AddDays(i); dt_nyce = dt_NYCE.AddDays(i);
                    dt_ends = dt_ENDS.AddDays(i); dt_ende = dt_ENDE.AddDays(i);
                    //Plot the Rectangles (BreakPeriods, PreHeatPeriods, PeakHours)
                    rt_DLit = Chart.DrawRectangle(pfx_AS1 + dt_as1s, dt_as1s, -9999, dt_as1e, 9999, cl_AS1, 0); rt_DLit.IsFilled = true;
                    rt_DLit = Chart.DrawRectangle(pfx_AS2 + dt_as2s, dt_as2s, -9999, dt_as2e, 9999, cl_AS2, 0); rt_DLit.IsFilled = true;
                    rt_DLit = Chart.DrawRectangle(pfx_EZ1 + dt_ez1s, dt_ez1s, -9999, dt_ez1e, 9999, cl_EZ1, 0); rt_DLit.IsFilled = true;
                    rt_DLit = Chart.DrawRectangle(pfx_EZ2 + dt_ez2s, dt_ez2s, -9999, dt_ez2e, 9999, cl_EZ2, 0); rt_DLit.IsFilled = true;
                    rt_DLit = Chart.DrawRectangle(pfx_NYE + dt_nyes, dt_nyes, -9999, dt_nyee, 9999, cl_NYE, 0); rt_DLit.IsFilled = true;
                    rt_DLit = Chart.DrawRectangle(pfx_NWT + dt_nwts, dt_nwts, -9999, dt_nwte, 9999, cl_NWT, 0); rt_DLit.IsFilled = true;
                    rt_DLit = Chart.DrawRectangle(pfx_LDF + dt_ldfs, dt_ldfs, -9999, dt_ldfe, 9999, cl_LDF, 0); rt_DLit.IsFilled = true;
                    rt_DLit = Chart.DrawRectangle(pfx_NYK + dt_nyks, dt_nyks, -9999, dt_nyke, 9999, cl_NYK, 0); rt_DLit.IsFilled = true;
                    rt_DLit = Chart.DrawRectangle(pfx_NYC + dt_nycs, dt_nycs, -9999, dt_nyce, 9999, cl_NYC, 0); rt_DLit.IsFilled = true;
                    rt_DLit = Chart.DrawRectangle(pfx_END + dt_ends, dt_ends, -9999, dt_ende, 9999, cl_END, 0); rt_DLit.IsFilled = true;
                    //Plot the Rectangles to indicator areas
                    for (int i_id = 0; i_id < Chart.IndicatorAreas.Count; i_id++)
                    {
                        rt_DLit = Chart.IndicatorAreas[i_id].DrawRectangle(pfx_AS1 + dt_as1s, dt_as1s, -9999, dt_as1e, 9999, cl_AS1, 0); rt_DLit.IsFilled = true;
                        rt_DLit = Chart.IndicatorAreas[i_id].DrawRectangle(pfx_AS2 + dt_as2s, dt_as2s, -9999, dt_as2e, 9999, cl_AS2, 0); rt_DLit.IsFilled = true;
                        rt_DLit = Chart.IndicatorAreas[i_id].DrawRectangle(pfx_EZ1 + dt_ez1s, dt_ez1s, -9999, dt_ez1e, 9999, cl_EZ1, 0); rt_DLit.IsFilled = true;
                        rt_DLit = Chart.IndicatorAreas[i_id].DrawRectangle(pfx_EZ2 + dt_ez2s, dt_ez2s, -9999, dt_ez2e, 9999, cl_EZ2, 0); rt_DLit.IsFilled = true;
                        rt_DLit = Chart.IndicatorAreas[i_id].DrawRectangle(pfx_NYE + dt_nyes, dt_nyes, -9999, dt_nyee, 9999, cl_NYE, 0); rt_DLit.IsFilled = true;
                        rt_DLit = Chart.IndicatorAreas[i_id].DrawRectangle(pfx_NWT + dt_nwts, dt_nwts, -9999, dt_nwte, 9999, cl_NWT, 0); rt_DLit.IsFilled = true;
                        rt_DLit = Chart.IndicatorAreas[i_id].DrawRectangle(pfx_LDF + dt_ldfs, dt_ldfs, -9999, dt_ldfe, 9999, cl_LDF, 0); rt_DLit.IsFilled = true;
                        rt_DLit = Chart.IndicatorAreas[i_id].DrawRectangle(pfx_NYK + dt_nyks, dt_nyks, -9999, dt_nyke, 9999, cl_NYK, 0); rt_DLit.IsFilled = true;
                        rt_DLit = Chart.IndicatorAreas[i_id].DrawRectangle(pfx_NYC + dt_nycs, dt_nycs, -9999, dt_nyce, 9999, cl_NYC, 0); rt_DLit.IsFilled = true;
                        rt_DLit = Chart.IndicatorAreas[i_id].DrawRectangle(pfx_END + dt_ends, dt_ends, -9999, dt_ende, 9999, cl_END, 0); rt_DLit.IsFilled = true;
                    }

                    //Plot DailyRoutineMarker
                    if (DailyRtMkr)
                    {
                        //Reset Ploting X-Position DateTime
                        dt_rnwd = dt_RNWD.AddDays(i);   if (dt_rnwd.DayOfWeek == DayOfWeek.Sunday) {dt_rnwd = dt_rnwd.AddDays(-2);}  //Move Start from Sunday evening to Friday evening;
                        dt_rjpx = dt_RJPX.AddDays(i);   dt_rcnx = dt_RCNX.AddDays(i);   dt_rcns = dt_RCNS.AddDays(i);   dt_rldo = dt_RLDO.AddDays(i);   dt_rasc = dt_RASC.AddDays(i);   dt_reso = dt_RESO.AddDays(i);
                        dt_rnyo = dt_RNYO.AddDays(i);   dt_rnyn = dt_RNYN.AddDays(i);   dt_rnxo = dt_RNXO.AddDays(i);   dt_rnys = dt_RNYS.AddDays(i);   dt_rnyd = dt_RNYD.AddDays(i);   dt_rwmr = dt_RWMR.AddDays(i);
                        dt_resc = dt_RESC.AddDays(i);   dt_rcax = dt_RCAX.AddDays(i);   dt_rldc = dt_RLDC.AddDays(i);   dt_rnya = dt_RNYA.AddDays(i);   dt_rnzr = dt_RNZR.AddDays(i);   dt_rnxc = dt_RNXC.AddDays(i);
                        dt_ecme = dt_RCME.AddDays(i);   dt_rnyc = dt_RNYC.AddDays(i);   dt_rapi = dt_RAPI.AddDays(i);   dt_reia = dt_REIA.AddDays(i);
                        
                        //Plot the DailyRoutineMarkers
                        tx_RMkr = Chart.DrawText(pfx_RNWD + dt_rnwd, mkr_RNWD, dt_rnwd, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        tx_RMkr = Chart.DrawText(pfx_RJPX + dt_rjpx, mkr_RJPX, dt_rjpx, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        tx_RMkr = Chart.DrawText(pfx_RCNX + dt_rcnx, mkr_RCNX, dt_rcnx, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        tx_RMkr = Chart.DrawText(pfx_RCNS + dt_rcns, mkr_RCNS, dt_rcns, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        tx_RMkr = Chart.DrawText(pfx_RLDO + dt_rldo, mkr_RLDO, dt_rldo, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        tx_RMkr = Chart.DrawText(pfx_RASC + dt_rasc, mkr_RASC, dt_rasc, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        tx_RMkr = Chart.DrawText(pfx_RESO + dt_reso, mkr_RESO, dt_reso, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        tx_RMkr = Chart.DrawText(pfx_RNYO + dt_rnyo, mkr_RNYO, dt_rnyo, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        tx_RMkr = Chart.DrawText(pfx_RNYN + dt_rnyn, mkr_RNYN, dt_rnyn, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        tx_RMkr = Chart.DrawText(pfx_RNXO + dt_rnxo, mkr_RNXO, dt_rnxo, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        tx_RMkr = Chart.DrawText(pfx_RNYS + dt_rnys, mkr_RNYS, dt_rnys, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        tx_RMkr = Chart.DrawText(pfx_RNYD + dt_rnyd, mkr_RNYD, dt_rnyd, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        tx_RMkr = Chart.DrawText(pfx_RWMR + dt_rwmr, mkr_RWMR, dt_rwmr, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        tx_RMkr = Chart.DrawText(pfx_RESC + dt_resc, mkr_RESC, dt_resc, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        tx_RMkr = Chart.DrawText(pfx_RLDC + dt_rldc, mkr_RLDC, dt_rldc, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        tx_RMkr = Chart.DrawText(pfx_RCAX + dt_rcax, mkr_RCAX, dt_rcax, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        tx_RMkr = Chart.DrawText(pfx_RNYA + dt_rnya, mkr_RNYA, dt_rnya, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        tx_RMkr = Chart.DrawText(pfx_RNZR + dt_rnzr, mkr_RNZR, dt_rnzr, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        tx_RMkr = Chart.DrawText(pfx_RNXC + dt_rnxc, mkr_RNXC, dt_rnxc, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        tx_RMkr = Chart.DrawText(pfx_RCME + dt_ecme, mkr_RCME, dt_ecme, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        tx_RMkr = Chart.DrawText(pfx_RNYC + dt_rnyc, mkr_RNYC, dt_rnyc, 0, cl_Rtn); ChTxtCenter(tx_RMkr);
                        if (dt_rapi.DayOfWeek == DayOfWeek.Tuesday  ) { tx_RMkr = Chart.DrawText(pfx_RAPI + dt_rapi, mkr_RAPI, dt_rapi, 0, cl_Rtn); ChTxtCenter(tx_RMkr); }
                        if (dt_reia.DayOfWeek == DayOfWeek.Wednesday) { tx_RMkr = Chart.DrawText(pfx_REIA + dt_reia, mkr_REIA, dt_reia, 0, cl_Rtn); ChTxtCenter(tx_RMkr); }

                    }

                }
                //Next Day
                i++;
            }

        }
        private void ChTxtCenter(ChartText tx_cnt)  //Set ChartText Horizontal,Vertical Center
        { tx_cnt.VerticalAlignment = VerticalAlignment.Center; tx_cnt.HorizontalAlignment = HorizontalAlignment.Center; tx_cnt.FontSize = 10; }

        //Plot MonthLighter on HalfYearChart
        private void PlotMthLight()
        {
            if (!MnthLit) return;
            ChartRectangle rt_Mnt;
            int yr = DayStart.Year;
            
            for (int iYr = yr-1; iYr <= yr+1; iYr++)
            {
                string sYr = iYr.ToString();
                rt_Mnt = Chart.DrawRectangle(sYr+"-Jan", new DateTime(iYr, 1, 1, 0, 0, 0), -9999, new DateTime(iYr, 2, 1, 0, 0, 0), 9999, Color.FromHex("103333FF"), 0, LineStyle.Solid); rt_Mnt.IsFilled = true;
                rt_Mnt = Chart.DrawRectangle(sYr+"-Feb", new DateTime(iYr, 2, 1, 0, 0, 0), -9999, new DateTime(iYr, 3, 1, 0, 0, 0), 9999, Color.FromHex("10AAAAAA"), 0, LineStyle.Solid); rt_Mnt.IsFilled = true;
                rt_Mnt = Chart.DrawRectangle(sYr+"-Mar", new DateTime(iYr, 3, 1, 0, 0, 0), -9999, new DateTime(iYr, 4, 1, 0, 0, 0), 9999, Color.FromHex("10AAFFAA"), 0, LineStyle.Solid); rt_Mnt.IsFilled = true;
                rt_Mnt = Chart.DrawRectangle(sYr+"-Apr", new DateTime(iYr, 4, 1, 0, 0, 0), -9999, new DateTime(iYr, 5, 1, 0, 0, 0), 9999, Color.FromHex("1033FF33"), 0, LineStyle.Solid); rt_Mnt.IsFilled = true;
                rt_Mnt = Chart.DrawRectangle(sYr+"-May", new DateTime(iYr, 5, 1, 0, 0, 0), -9999, new DateTime(iYr, 6, 1, 0, 0, 0), 9999, Color.FromHex("1000FFAA"), 0, LineStyle.Solid); rt_Mnt.IsFilled = true;
                rt_Mnt = Chart.DrawRectangle(sYr+"-Jun", new DateTime(iYr, 6, 1, 0, 0, 0), -9999, new DateTime(iYr, 7, 1, 0, 0, 0), 9999, Color.FromHex("1055FFFF"), 0, LineStyle.Solid); rt_Mnt.IsFilled = true;
                rt_Mnt = Chart.DrawRectangle(sYr+"-Jul", new DateTime(iYr, 7, 1, 0, 0, 0), -9999, new DateTime(iYr, 8, 1, 0, 0, 0), 9999, Color.FromHex("10AAEE55"), 0, LineStyle.Solid); rt_Mnt.IsFilled = true;
                rt_Mnt = Chart.DrawRectangle(sYr+"-Aug", new DateTime(iYr, 8, 1, 0, 0, 0), -9999, new DateTime(iYr, 9, 1, 0, 0, 0), 9999, Color.FromHex("10FFFF00"), 0, LineStyle.Solid); rt_Mnt.IsFilled = true;
                rt_Mnt = Chart.DrawRectangle(sYr+"-Sep", new DateTime(iYr, 9, 1, 0, 0, 0), -9999, new DateTime(iYr,10, 1, 0, 0, 0), 9999, Color.FromHex("10FF9900"), 0, LineStyle.Solid); rt_Mnt.IsFilled = true;
                rt_Mnt = Chart.DrawRectangle(sYr+"-Oct", new DateTime(iYr,10, 1, 0, 0, 0), -9999, new DateTime(iYr,11, 1, 0, 0, 0), 9999, Color.FromHex("10FF3377"), 0, LineStyle.Solid); rt_Mnt.IsFilled = true;
                rt_Mnt = Chart.DrawRectangle(sYr+"-Nov", new DateTime(iYr,11, 1, 0, 0, 0), -9999, new DateTime(iYr,12, 1, 0, 0, 0), 9999, Color.FromHex("10EE11AA"), 0, LineStyle.Solid); rt_Mnt.IsFilled = true;
                rt_Mnt = Chart.DrawRectangle(sYr+"-Dec", new DateTime(iYr,12, 1, 0, 0, 0), -9999, new DateTime(iYr,12,31,23,59,59), 9999, Color.FromHex("10AAAAFF"), 0, LineStyle.Solid); rt_Mnt.IsFilled = true;
            }
        }

        //Plot 5Mins, 1-4Hour on(Today and Yesterday) <=80Tick, 250Tick, 1000Tick TimeFrame
        private void DrawIntradayVertLnes()
        {
            if (!MinHorLne) return;
            string TFGroup = new string(TimeFrame.ToString().Take(4).ToArray()); //Get TimeFrame GroupName
            if (TFGroup == "Tick" && TimeFrame <= TimeFrame.Tick80)
            {
                for (DateTime currentTime = DayStart; currentTime <= DayEnd; currentTime = currentTime.AddMinutes(5))
                {
                    DateTime dt5MinMkr = currentTime; string s5MinMkr = string.Format("5Min_VertLnes {0}", dt5MinMkr);  //Time of 5Min Seperator Marker, Line ObjectName
                    Chart.DrawVerticalLine(s5MinMkr, dt5MinMkr, clrTikMkr(DayStart, dt5MinMkr), 1, LineStyle.Dots);
                }
            }
            else if (TimeFrame.ToString() == "Tick250" || TimeFrame.ToString() == "Minute5")
            {
                for (DateTime currentTime = DayStart; currentTime <= DayEnd; currentTime = currentTime.AddHours(1))
                {
                    DateTime dt1hrMkr = currentTime; string s1hrMkr = string.Format("1Hour_VertLnes {0}", dt1hrMkr);    //Time of 1hour Seperator Marker, Line ObjectName
                    Chart.DrawVerticalLine(s1hrMkr, dt1hrMkr, clrTikMkr(DayStart, dt1hrMkr), 1, LineStyle.Solid);
                }
            }
            else if (TimeFrame.ToString() == "Tick1000")
            {
                for (DateTime currentTime = LstStart; currentTime <= DayStart; currentTime = currentTime.AddHours(4))   //2 Days ago
                {
                    DateTime dt4hrMkr = currentTime; string s4hrMkr = string.Format("4Hour_VertLnes {0}", dt4hrMkr);    //Time of 4Hour Seperator Marker, Line ObjectName
                    Chart.DrawVerticalLine(s4hrMkr, dt4hrMkr, clrTikMkr(LstStart, dt4hrMkr), 1, LineStyle.Solid);
                }
                for (DateTime currentTime = DayStart; currentTime <= DayEnd; currentTime = currentTime.AddHours(4))     //1 Day ago
                {
                    DateTime dt4hrMkr = currentTime; string s4hrMkr = string.Format("4Hour_VertLnes {0}", dt4hrMkr);    //Time of 4Hour Seperator Marker, Line ObjectName
                    Chart.DrawVerticalLine(s4hrMkr, dt4hrMkr, clrTikMkr(DayStart, dt4hrMkr), 1, LineStyle.Solid);
                }
            }
        }
        //Define 5Min 1-4Hour Seperator's Colour
        private Color clrTikMkr(DateTime dt_St, DateTime dtTMkr) 
        {
            if      (dtTMkr == dt_St              || dtTMkr == dt_St.AddHours( 1) || dtTMkr == dt_St.AddHours( 2) || dtTMkr == dt_St.AddHours( 3) ) { return Color.FromHex("#667788FF"); } //DarkBlue
            else if (dtTMkr == dt_St.AddHours( 4) || dtTMkr == dt_St.AddHours( 5) || dtTMkr == dt_St.AddHours( 6) || dtTMkr == dt_St.AddHours( 7) ) { return Color.FromHex("#AA7788FF"); } //LightBlue

            else if (dtTMkr == dt_St.AddHours( 8) || dtTMkr == dt_St.AddHours( 9) || dtTMkr == dt_St.AddHours(10) || dtTMkr == dt_St.AddHours(11) ) { return Color.FromHex("#AA99FF99"); } //MorningGreen
            else if (dtTMkr == dt_St.AddHours(12) || dtTMkr == dt_St.AddHours(13) || dtTMkr == dt_St.AddHours(14) || dtTMkr == dt_St.AddHours(15) ) { return Color.FromHex("#6699FF99"); } //MorningNoon

            else if (dtTMkr == dt_St.AddHours(16) || dtTMkr == dt_St.AddHours(17) || dtTMkr == dt_St.AddHours(18) || dtTMkr == dt_St.AddHours(19) ) { return Color.FromHex("#AAFF7777"); } //AfternoonFight
            else if (dtTMkr == dt_St.AddHours(20) || dtTMkr == dt_St.AddHours(21) || dtTMkr == dt_St.AddHours(22) || dtTMkr == dt_St.AddHours(23) ) { return Color.FromHex("#66FF7777"); } //AfternoonToEnd

            else { return Color.FromHex("#15FFFFFF"); } //5min Grey
        }

    }

}