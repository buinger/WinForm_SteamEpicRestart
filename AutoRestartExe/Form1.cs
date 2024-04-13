using Microsoft.Win32;
using System.Diagnostics;

namespace AutoRestartExe
{
    public partial class Form1 : Form
    {
        string[] lastDates;
        string steamPath = "";
        string epicPath = "";
        public Form1()
        {
            InitializeComponent();
            string targetData = LoadDataFromFile();
            //Debug.WriteLine("----------------------!!!!!!!!!");
            if (targetData != "")
            {
                lastDates = targetData.Split('*');
                string[] datas = lastDates;


                runTime_Input.Text = datas[1];
                waitTime_Input.Text = datas[2];

                if (datas[5] == "False")
                {
                    proName_Input.Text = datas[0];
                    gameIdToogle.Checked = false;
                    gameId_Input.Text = datas[3];
                }
                else
                {
                    proName_Input.Text = datas[6];
                    gameIdToogle.Checked = true;
                    gameId_Input.Text = datas[4];
                }
            }


        }


        void StartGame(string id)
        {
            if (gameIdToogle.Checked == false)
            {
                // ���� Steam ��Ϸ��������
                string steamParams = "-applaunch " + id;
                // ���� Steam ��������Ϸ
                Process.Start(steamPath, steamParams);

            }
            else
            {
                // ����Ҫ��������Ϸ��Ӧ�ó���ID��URL scheme
                string gameAppIdOrURL = "com.epicgames.launcher://apps/" + id + "?action=launch&silent=true";

                // ʹ��Process����Epic Games�ͻ��ˣ���������ϷӦ�ó���ID��URL scheme��Ϊ����
                ProcessStartInfo startInfo = new ProcessStartInfo(epicPath, gameAppIdOrURL);
                Process.Start(startInfo);

            }
        }


        Task jianKong;
        bool jianKongSwitch = false;


        async Task DoJianKong(int rSecond, int wSecond)
        {
            infoBox.Text = "��ʼ���";
            proName_Input.ReadOnly = true;
            gameId_Input.ReadOnly = true;
            runTime_Input.ReadOnly = true;
            waitTime_Input.ReadOnly = true;
            gameIdToogle.Enabled = false;

            jianKongSwitch = true;
            startButton.Text = "���ڼ��(���ֹͣ)";
            startButton.ForeColor = Color.Green;

            int fullPass = 0, runPass = 0, waitPass = 0;
            while (jianKongSwitch)
            {
                await Task.Delay(1000);
                fullPass += 1;

                //infoBox.Text = "����ʱ��:" + fullPass + "��\n";
                //infoBox.Text = "";
                if (CheckIsRun())
                {
                    infoBox.Text = "��Ϸ������\n";
                    runPass += 1;


                    infoBox.Text += "����ʱ��:" + GetTimeStr(runPass) + "\n";


                    infoBox.Text += "��ʣ" + GetTimeStr(rSecond - runPass) + "�ر���Ϸ";
                    if (runPass >= rSecond)
                    {
                        runPass = 0;
                        waitPass = 0;
                        KillPro();
                    }
                }
                else
                {
                    //if (runPass != 0)
                    //{
                    //    infoBox.Text += "��⵽��Ϸ��������������������";
                    //    StartGame(gameId_Input.Text);
                    //    runPass = 0;
                    //    waitPass = 0;
                    //    await Task.Delay(3000);
                    //}
                    //else
                    //{
                    infoBox.Text = "��Ϸ�ѹر�\n";
                    waitPass += 1;


                    infoBox.Text += "��Ϣʱ��:" + GetTimeStr(waitPass) + "\n";


                    infoBox.Text += "��ʣ" + GetTimeStr(wSecond - waitPass) + "������Ϸ";
                    if (waitPass >= wSecond)
                    {
                        runPass = 0;
                        waitPass = 0;
                        KillPro();
                        StartGame(gameId_Input.Text);
                    }
                    //}

                }



            }
            infoBox.Text = "";
        }


        string GetTimeStr(int fullSec)
        {
            int min = fullSec / 60;
            int sec = fullSec % 60;
            if (min == 0)
            {
                return sec + "��";
            }
            else
            {
                return min + "��" + sec + "��";
            }

        }

        bool CheckIsRun()
        {
            Process[] processes = Process.GetProcessesByName(proName_Input.Text);
            bool isRun = processes.Length >= 1;

            return isRun;
        }

        void KillPro()
        {
            Process[] processes = Process.GetProcessesByName(proName_Input.Text);
            foreach (var item in processes)
            {
                item.Kill();
            }
        }


        void EndJianKong()
        {
            jianKongSwitch = false;
            startButton.Text = "��ʼ���";
            startButton.ForeColor = Color.Black;
            proName_Input.ReadOnly = false;
            gameId_Input.ReadOnly = false;
            runTime_Input.ReadOnly = false;
            waitTime_Input.ReadOnly = false;
            gameIdToogle.Enabled = true;
        }






        void CheckAndDoJianKong()
        {

            RemeberGameIdAndName();

            if (jianKong != null)
            {
                if (!jianKong.IsCompleted)
                {
                    MessageBox.Show("��ؽ��̻������У����Ժ�����");
                    return;
                }
            }
            int rTime, wTime;
            if (!int.TryParse(runTime_Input.Text, out rTime))
            {
                MessageBox.Show("����ʱ����д��������");
                return;
            }

            if (!int.TryParse(waitTime_Input.Text, out wTime))
            {
                MessageBox.Show("��Ϣʱ����д��������");
                return;
            }



            if (proName_Input.Text != "")
            {

                if (gameIdToogle.Checked == false)
                {
                    if (!SteamGameCheck())
                    {
                        return;
                    }
                }
                else
                {
                    if (!EpicGameCheck())
                    {
                        return;
                    }
                }



                if (CheckIsRun())
                {
                    jianKong = DoJianKong(rTime * 60, wTime * 60);
                }
                else
                {
                    MessageBox.Show(proName_Input.Text + ".exe��δ���У������к��ٵ����ʼ");
                    return;
                }

            }
            else
            {
                MessageBox.Show("����������Ϊ��");
                return;
            }


            void RemeberGameIdAndName()
            {
                if (gameIdToogle.Checked == true)
                {
                    lastDates[6] = proName_Input.Text;
                    lastDates[4] = gameId_Input.Text;
                }
                else
                {
                    lastDates[0] = proName_Input.Text;
                    lastDates[3] = gameId_Input.Text;
                }
            }

            bool SteamGameCheck()
            {

                if (steamPath == "")
                {
                    Process[] processes = Process.GetProcessesByName("Steam");
                    if (processes.Length == 0)
                    {
                        MessageBox.Show("δ��⵽steam����");
                        return false;
                    }
                    steamPath = processes[0]?.MainModule?.FileName;
                }

                Process[] allGamePro = Process.GetProcessesByName(proName_Input.Text);
                if (allGamePro.Length != 0)
                {
                    Process steamGameProcess = allGamePro[0];
                    // ���� Steam ��Ϸ���̵�����·������ "steamapps\common"
                    string fullName = "";
                    if (steamGameProcess.MainModule != null)
                    {
                        fullName = steamGameProcess.MainModule.FileName != null ? steamGameProcess.MainModule.FileName : "";
                    }
                    if (fullName == "")
                    {
                        MessageBox.Show("δ�ҵ���Ϸ��·����������Ϸ���Ƿ���ȷ");
                        return false;
                    }


                    //if (!fullName.Contains("steamapps\\common"))
                    //{

                    //    MessageBox.Show(proName_Input.Text + ".exe������steam��Ϸ���޷����");
                    //    return false;
                    //}
                }
                else
                {
                    MessageBox.Show(proName_Input.Text + ".exe��δ���У������к��ڵ����ʼ");
                    return false;
                }

                return true;
            }



            bool EpicGameCheck()
            {
                // ��� Epic Games �ͻ���·��Ϊ�գ����Բ��Ҳ�����
                if (epicPath == "")
                {
                    Process[] processes = Process.GetProcessesByName("EpicGamesLauncher");
                    if (processes.Length == 0)
                    {
                        MessageBox.Show("δ��⵽ Epic Games �ͻ��˳���");
                        return false;
                    }
                    epicPath = processes[0]?.MainModule?.FileName;
                }

                // ��ȡָ����Ϸ����
                Process[] allGamePro = Process.GetProcessesByName(proName_Input.Text);

                if (allGamePro.Length != 0)
                {
                    Process epicGameProcess = allGamePro[0];
                    // ���� Epic Games ��Ϸ���̵�����·������ "Epic Games"
                    string fullName = "";
                    if (epicGameProcess.MainModule != null)
                    {
                        fullName = epicGameProcess.MainModule.FileName != null ? epicGameProcess.MainModule.FileName : "";
                    }
                    if (fullName == "")
                    {
                        MessageBox.Show("δ�ҵ���Ϸ��·����������Ϸ���Ƿ���ȷ");
                        return false;
                    }

                    //if (!fullName.Contains("Epic Games"))
                    //{
                    //    Debug.WriteLine(fullName);
                    //    MessageBox.Show(proName_Input.Text + ".exe������ Epic Games ��Ϸ���޷����");
                    //    return false;
                    //}
                }
                else
                {
                    MessageBox.Show(proName_Input.Text + ".exe��δ���У������к��ٵ����ʼ");
                    return false;
                }

                return true;
            }

        }


        private void StartButton_Click(object sender, EventArgs e)
        {
            if (startButton.Text != "���ڼ��(���ֹͣ)")
            {
                CheckAndDoJianKong();
            }
            else
            {
                EndJianKong();
            }

        }



        //"Ĭ������.txt"
        private string GetDataFilePath()
        {
            // ��ȡ��ǰ���г����·������ƴ�������ļ���
            return Path.Combine(Application.StartupPath, "��������.txt");
        }

        private void SaveDataToFile(string data)
        {
            try
            {
                // ��ȡ�����ļ�·��
                string filePath = GetDataFilePath();

                // ����ļ������ڣ��򴴽��ļ�
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close(); // �����ļ��������رգ��������������̷���
                }

                // ������д���ļ�
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(data);
                }
                //MessageBox.Show("���ݱ���ɹ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��������ʱ����{ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string LoadDataFromFile()
        {
            try
            {
                // ��ȡ�����ļ�·��
                string filePath = GetDataFilePath();

                // ����ļ������ڣ��򷵻ؿ��ַ���
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close(); // �����ļ��������رգ��������������̷���
                    SaveDataToFile("*****False*");
                }

                // ���ļ��ж�ȡ����
                using (StreamReader reader = new StreamReader(filePath))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��������ʱ����{ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (gameIdToogle.Checked == false)
            {
                SaveDataToFile(lastDates[0] + "*" + runTime_Input.Text + "*" + waitTime_Input.Text + "*" + gameId_Input.Text + "*" + lastDates[4] + "*" + false + "*" + lastDates[6]);
            }
            else
            {
                SaveDataToFile(lastDates[0] + "*" + runTime_Input.Text + "*" + waitTime_Input.Text + "*" + lastDates[3] + "*" + gameId_Input.Text + "*" + true + "*" + lastDates[6]);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (gameIdToogle.Checked == true)
            {
                gameIdToogle.Text = "Epic��ϷID";
                gameId_Input.Text = lastDates[4];
                proName_Input.Text = lastDates[6];
            }
            else
            {
                gameIdToogle.Text = "Steam��ϷID";
                gameId_Input.Text = lastDates[3];
                proName_Input.Text = lastDates[0];
            }
        }
    }
}