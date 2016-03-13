using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sample.Comm;
using Sample.Data.DBHepler;
using Sample.Model;

namespace Sample.Client.Hepler
{
    public class MessageProvider : IItemsProvider<MessageModel>
    {
        private int _count;
        private readonly int _fetchDelay;

        public DBHelper DbHelper { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProvider"/> class.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="fetchDelay">The fetch delay.</param>
        public MessageProvider(int count, int fetchDelay)
        {
            _count = count;
            _fetchDelay = fetchDelay;
        }

        /// <summary>
        /// Fetches the total number of items available.
        /// </summary>
        /// <returns></returns>
        public int FetchCount()
        {
            return _count;
        }

        /// <summary>
        /// Fetches a range of items.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The number of items to fetch.</param>
        /// <param name="overallCount"></param>
        /// <returns></returns>
        public IList<MessageModel> FetchRange(int startIndex, int count)
        {
            Debug.WriteLine(String.Format("start index: {0}", startIndex));
            return DbOperator.GetMessages(DbHelper, startIndex, count);
        }

        /// <summary>
        /// Pretend to insert an item.
        /// </summary>
        public void InsertItem()
        {
            _count++;
        }

        /// <summary>
        /// Pretend to remove an item.
        /// </summary>
        public void RemoveItem()
        {
            _count--;
        }
    }
}
