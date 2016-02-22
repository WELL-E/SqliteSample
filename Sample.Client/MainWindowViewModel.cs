using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Input;
using Sample.Comm;
using Sample.Data;
using Sample.Data.SqliteHepler;
using Sample.Model;

namespace Sample.Client
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private DBHelper _dbHelper;
        private Timer _timer;
        private MessageModel _selectedMsg;

        public MainWindowViewModel()
        {
            InitDB();
        }

        private void InitDB()
        {
            var path = Environment.CurrentDirectory + @"\SampleData\sample";

            var clientConnStr = new SqlConnectionStringBuilder
            {
                DataSource = path,
                IntegratedSecurity = false,
                MinPoolSize = 1,
                MaxPoolSize = 5,
                ConnectTimeout = 10
            };

            _dbHelper = new SqliteHelper(clientConnStr);
            _timer = new Timer(GetMemoryCallBack, null, 0, 1000);
        }

        private ObservableCollection<MessageModel> _messagesList; 

        public ObservableCollection<MessageModel> MessagesList
        {
            get
            {
                if (_messagesList == null)
                {
                    _messagesList = new ObservableCollection<MessageModel>();
                }
                return _messagesList;
            }
        }

        private int _msgCount;

        public int MsgCount
        {
            get { return _msgCount; }
            set
            {
                _msgCount = value;
                OnPropertyChanged("MsgCount");
            }
        }

        private string _memorySize;

        public string MemorySize
        {
            get { return _memorySize; }
            private set
            {
                _memorySize = value;
                OnPropertyChanged("MemorySize");
            }
        }

        private string _totalTime;

        public string TotalTime
        {
            get { return _totalTime; }
            set
            {
                _totalTime = value;
                OnPropertyChanged("TotalTime");
            }
        }

        private ICommand _loadMsgCmd;

        public ICommand LoadMsgCmd
        {
            get
            {
                if (_loadMsgCmd == null)
                {
                    _loadMsgCmd = new RelayCommand(InvokeLoadMsg);
                }

                return _loadMsgCmd;
            }
        }

        private DelegateCommand _deleteItemCmd;

        public DelegateCommand DeleteItemCmd
        {
            get { return _deleteItemCmd ?? (_deleteItemCmd = new DelegateCommand(InvokeDeleteItem)); }
            set { _deleteItemCmd = value; }
        }

        private DelegateCommand<ExCommandParameter> _selectionChangedCmd;

        public DelegateCommand<ExCommandParameter> SelectionChangedCmd
        {
            get {
                return _selectionChangedCmd ??
                       (_selectionChangedCmd = new DelegateCommand<ExCommandParameter>(InvokeSelectionChanged));
            }
            set { _selectionChangedCmd = value; }
        }

        private void InvokeLoadMsg(object obj)
        {
            var watcher = new Stopwatch();
            watcher.Start();
            var ds = _dbHelper.GetDataSet("select * from messages");
            watcher.Stop();
            TotalTime = watcher.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var msg = new MessageModel
                {
                    MsgId = row["MsgId"].ToString(),
                    Title = row["Title"].ToString(),
                    Sender = row["Sender"].ToString(),
                    Receiver = row["Receiver"].ToString(),
                    RecvTime = row["RecvTime"].ToString(),
                    Body = row["Body"].ToString(),
                    IsRead = Convert.ToBoolean(row["IsRead"]) 
                };
                _messagesList.Add(msg);
            }
        }

        private void InvokeDeleteItem()
        {
            MessagesList.Remove(_selectedMsg);
        }

        private void InvokeSelectionChanged(ExCommandParameter param)
        {
            var item = param.Parameter as MessageModel;
            if (item == null) return;
            _selectedMsg = item;
        }

        private void GetMemoryCallBack(object obj)
        {
            MemorySize = String.Format("{0:0.00} MB", GC.GetTotalMemory(true) / 1024.0 / 1024.0);
        }
    }
}
