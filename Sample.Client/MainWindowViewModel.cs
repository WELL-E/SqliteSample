using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Input;
using Sample.Client.Hepler;
using Sample.Comm;
using Sample.Data.DBHepler;
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

        //private VirtualizingList<MessageModel> _messagesList; 

        public VirtualizingList<MessageModel> MessagesList
        {
            get
            {
                return InvokeLoadMsg(); 
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

        /// <summary>
        /// 增加一条数据命令
        /// </summary>
        private DelegateCommand _addItemCmd;

        public DelegateCommand AddItemCmd
        {
            get { return _addItemCmd ?? (_addItemCmd = new DelegateCommand(InvokeAddItem)); }
            set { _addItemCmd = value; }
        }


        /// <summary>
        /// 删除数据命令
        /// </summary>
        private DelegateCommand _deleteItemCmd;

        public DelegateCommand DeleteItemCmd
        {
            get { return _deleteItemCmd ?? (_deleteItemCmd = new DelegateCommand(InvokeDeleteItem)); }
            set { _deleteItemCmd = value; }
        }

        /// <summary>
        /// 选择数据项改变命令
        /// </summary>
        private DelegateCommand<ExCommandParameter> _mouseDownCmd;

        public DelegateCommand<ExCommandParameter> MouseDownCmd
        {
            get {
                return _mouseDownCmd ??
                       (_mouseDownCmd = new DelegateCommand<ExCommandParameter>(InvokeSelectedItem));
            }
            set { _mouseDownCmd = value; }
        }

        private VirtualizingList<MessageModel>  InvokeLoadMsg()
        {
            var watcher = new Stopwatch();
            watcher.Start();
            //邮件总数
            var count = DbOperator.GetMessageCount(_dbHelper);

            //获取邮件
            var mailProvider = new MessageProvider(count, 1000) { DbHelper = _dbHelper };

            var list = new VirtualizingList<MessageModel>(mailProvider, 100, 1000);
            watcher.Stop();
            TotalTime = watcher.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture);

            return list;
        }

        private void InvokeAddItem()
        {
            
        }

        private void InvokeDeleteItem()
        {
            MessagesList.Remove(_selectedMsg);
        }

        private void InvokeSelectedItem(ExCommandParameter param)
        {
            var item = param.Parameter as MessageModel;
            if (item == null) return;
            _selectedMsg = item;

            _selectedMsg.IsRead = true;
            OnPropertyChanged("IsRead");
        }

        private void GetMemoryCallBack(object obj)
        {
            MemorySize = String.Format("{0:0.00} MB", GC.GetTotalMemory(true) / 1024.0 / 1024.0);
        }
    }
}
