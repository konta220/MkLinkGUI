using System;
using System.Windows;

namespace MklinkGUI
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void actionButton_Click(object sender, RoutedEventArgs e)
        {
            CmdStart((String)path1_label.Content, (String)path2_label.Content);

        }

        private void CmdStart(string real, string link)
        {
            resultTextBox.Clear();

            if (real.Equals(link))
            {
                setLogger("同じフォルダには指定できません。");
            }

            if (!System.IO.Directory.Exists(real))
            {
                setLogger("指定した「実在するフォルダ」が存在しません。");
                setLogger(real);
                return;
            }

            if (System.IO.Directory.Exists(link))
            {
                link += "\\" + System.IO.Path.GetFileName(real);
            }

            if (System.IO.Directory.Exists(link))
            {
                setLogger("リンク先のフォルダに同様のフォルダがあります。");
                setLogger(link);
                return;
            }
            else
            {
                string linkPath = System.IO.Directory.GetParent(link).FullName;

                if (!System.IO.Directory.Exists(linkPath))
                {
                    setLogger("指定した「リンクを作る親のフォルダ」が存在しません。");
                    setLogger(linkPath);
                    return;
                }
            }

            string command = "MKLINK /d \"" + link + "\" \"" + real + "\"";

            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = Environment.GetEnvironmentVariable("ComSpec");
            proc.StartInfo.Verb = "RunAs";

            proc.StartInfo.Arguments = @"/c "+command;

            setLogger(command);

            try
            {
                proc.Start();
            }
            catch (System.ComponentModel.Win32Exception)
            {
                //「ユーザーアカウント制御」ダイアログでキャンセルされたなどによって
                //起動できなかった時
                setLogger("管理者権限で実行してください。");
                return;
            }

            setLogger("作成完了");
        }

        private void setLogger(string message)
        {
            resultTextBox.Text += message + Environment.NewLine;
            resultTextBox.ScrollToEnd();
        }

        private void path1Button_Click(object sender, RoutedEventArgs e)
        {
            string folferPath = GetFolderPath();
            if (folferPath != null)
            {
                path1.Text = folferPath;
            }
        }

        private void path2Button_Click(object sender, RoutedEventArgs e)
        {
            string folferPath = GetFolderPath();
            if (folferPath != null)
            {
                path2.Text = folferPath;
            }
        }

        private string GetFolderPath()
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();

            fbd.Description = "フォルダを指定してください。";

            fbd.RootFolder = Environment.SpecialFolder.Desktop;

            fbd.ShowNewFolderButton = true;

            //ダイアログを表示する
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //選択されたフォルダ返す
                return fbd.SelectedPath;
            }

            return null;
        }

        private void path1_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (path1_label != null)
                path1_label.Content = path1.Text;
        }

        private void path2_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (path2_label != null)
                path2_label.Content = path2.Text;
        }

        private void path1_Drag_Drop(object sender, System.Windows.DragEventArgs e)
        {

            // ドロップされた情報からファイルパスをゲット
            foreach (string strFileName in (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop))
            {
                // ファイルパスからファイル情報をゲット
                System.IO.FileInfo fi = new System.IO.FileInfo(strFileName);

                // ファイルパス + (サイズ) の文字列をテキストボックスに追加
                path1.Text = strFileName;
            }
        }

        private void path2_Drag_Drop(object sender, System.Windows.DragEventArgs e)
        {
            // ドロップされた情報からファイルパスをゲット
            foreach (string strFileName in (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop))
            {
                // ファイルパスからファイル情報をゲット
                System.IO.FileInfo fi = new System.IO.FileInfo(strFileName);

                // ファイルパス + (サイズ) の文字列をテキストボックスに追加
                path2.Text = strFileName;
            }
        }

        private void path1_Drag_DragOver(object sender, System.Windows.DragEventArgs e)
        {
            e.Effects = System.Windows.DragDropEffects.Copy;
            e.Handled = true;
        }

        private void path2_Drag_DragOver(object sender, System.Windows.DragEventArgs e)
        {
            e.Effects = System.Windows.DragDropEffects.Copy;
            e.Handled = true;
        }
    }
}