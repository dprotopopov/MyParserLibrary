using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using MyLibrary.Lock;

namespace MyParser.Forms
{
    public class DataGridViewResultManager<T> : ISingleLock<BindingList<T>>, ISingleLock<GridControl>
    {
        private readonly Dictionary<string, int> _lviItemsIndex = new Dictionary<string, int>();

        public DataGridViewResultManager()
        {
            MutexItems = new Mutex();
            MutexDataGridView = new Mutex();
            Items = new BindingList<T>();
        }

        public IManagerProvider ChildForm { get; set; }
        private Mutex MutexItems { get; set; }
        private Mutex MutexDataGridView { get; set; }

        public BindingList<T> Items { get; set; }
        public GridControl DataGridView { get; set; }

        public bool Wait(BindingList<T> semaphore)
        {
            while (!MutexItems.WaitOne(0))
            {
                Application.DoEvents();
                Thread.Yield();
            }
            return true;
        }

        public bool Wait(BindingList<T> semaphore, TimeSpan timeout)
        {
            return MutexItems.WaitOne(timeout);
        }

        public void Release(BindingList<T> semaphore)
        {
            MutexItems.ReleaseMutex();
        }

        public bool Wait(GridControl semaphore)
        {
            while (!MutexDataGridView.WaitOne(0))
            {
                Application.DoEvents();
                Thread.Yield();
            }
            return true;
        }

        public bool Wait(GridControl semaphore, TimeSpan timeout)
        {
            return MutexDataGridView.WaitOne(timeout);
        }

        public void Release(GridControl semaphore)
        {
            MutexDataGridView.ReleaseMutex();
        }
    }
}