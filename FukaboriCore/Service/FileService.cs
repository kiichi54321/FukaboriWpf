using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FukaboriCore.Service
{
    public interface IFileService
    {
        Task Save(string filename, string defaultExt, string saveText);
        Task Save(string filename, string defaultExt, Action<Stream> saveAction);
        Task<string> Load(string filename, string defaultExt);
        Task<T> Load<T>(string filename, string defaultExt, Func<Stream,Task<T>> loadFunc);
        Task<bool> Load(string filename, string defaultExt, Func<Stream, Task> loadAction);
    }

    public class FileService : IFileService
    {
        public async Task<string> Load(string filename, string defaultExt)
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                FileName = filename,
                RestoreDirectory = true,
                DefaultExt = defaultExt,
                Filter = $"{defaultExt} Files|*{defaultExt}|All Files|*.*",
                FilterIndex = 0
            };
            if (fileDialog.ShowDialog() == true)
            {
                try
                {
                    var sr = new System.IO.StreamReader(fileDialog.OpenFile());
                    var text = await sr.ReadToEndAsync();
                    System.Windows.MessageBox.Show("完了");
                    return text;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("失敗" + ex.ToString());
                }
            }
            return null;
        }

        public async Task<T> Load<T>(string filename, string defaultExt, Func<Stream, Task<T>> loadFunc)
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                FileName = filename,
                RestoreDirectory = true,
                DefaultExt = defaultExt,
                Filter = $"{defaultExt} Files|*{defaultExt}|All Files|*.*",
                FilterIndex = 0
            };
            if (fileDialog.ShowDialog() == true)
            {
                try
                {
                    var result = await loadFunc(fileDialog.OpenFile());
                    System.Windows.MessageBox.Show("完了");
                    return result;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("失敗" + ex.ToString());
                }
            }
            return default(T);
        }

        public async Task<bool> Load(string filename, string defaultExt, Func<Stream,Task> loadAction)
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                FileName = filename,
                RestoreDirectory = true,
                DefaultExt = defaultExt,
                Filter = $"{defaultExt} Files|*{defaultExt}|All Files|*.*",
                FilterIndex = 0
            };
            if (fileDialog.ShowDialog() == true)
            {
                try
                {
                    await loadAction(fileDialog.OpenFile());
                    System.Windows.MessageBox.Show("完了");
                    return true;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("失敗" + ex.ToString());
                }
            }
            return false;
        }

        public Task Save(string filename, string defaultExt, string saveText)
        {
            if (string.IsNullOrEmpty(saveText))
            {
                System.Windows.MessageBox.Show("保存する内容がありません。");
                return Task.CompletedTask;
            }
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                FileName = filename,
                RestoreDirectory = true,
                DefaultExt = defaultExt,
                Filter = $"{defaultExt} Files|*{defaultExt}|All Files|*.*",
                FilterIndex = 0
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    System.IO.File.WriteAllText(saveFileDialog.FileName, saveText);
                    System.Windows.MessageBox.Show("完了");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("失敗" + ex.ToString());
                }
            }
            return Task.CompletedTask;
        }

        public Task Save(string filename, string defaultExt, Action<Stream> saveAction)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                FileName = filename,
                RestoreDirectory = true,
                DefaultExt = defaultExt,
                Filter = $"{defaultExt} Files|*{defaultExt}|All Files|*.*",
                FilterIndex = 0
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (var stream = saveFileDialog.OpenFile())
                    {
                        saveAction(stream);
                        System.Windows.MessageBox.Show("完了");
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("失敗" + ex.ToString());
                }
            }
            return Task.CompletedTask;
        }


    }
}
