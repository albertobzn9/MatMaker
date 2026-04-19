using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Data.Matlab;

namespace CreadorMatWin
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form form = new Form
            {
                Text = "Excel a MAT - Convertidor",
                Width = 700,
                Height = 550,
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = Color.FromArgb(245, 245, 247),
                Font = new Font("Segoe UI", 10)
            };

            Panel panelTop = new Panel { Dock = DockStyle.Top, Height = 80, Padding = new Padding(20) };
            TextBox txtVarName = new TextBox { Dock = DockStyle.Top, Font = new Font("Segoe UI", 12) };
            Label lblVar = new Label { Text = "1. Nombre de la variable (Ej. exp_0126_dis_d1r1):", Dock = DockStyle.Top, Height = 25, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            panelTop.Controls.Add(txtVarName);
            panelTop.Controls.Add(lblVar);

            Panel panelMid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20, 0, 20, 0) };
            TextBox txtData = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Both, WordWrap = false, Font = new Font("Consolas", 10) };
            Label lblData = new Label { Text = "2. Pega aquí tus datos de Excel (Ctrl + V):", Dock = DockStyle.Top, Height = 25, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            panelMid.Controls.Add(txtData);
            panelMid.Controls.Add(lblData);

            Panel panelBot = new Panel { Dock = DockStyle.Bottom, Height = 100, Padding = new Padding(20) };
            Label lblStatus = new Label { Text = "", Dock = DockStyle.Top, Height = 30, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            Button btnSave = new Button { Text = "Guardar como .mat", Dock = DockStyle.Top, Height = 40, BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 11, FontStyle.Bold), Cursor = Cursors.Hand };
            btnSave.FlatAppearance.BorderSize = 0;
            panelBot.Controls.Add(lblStatus);
            panelBot.Controls.Add(btnSave);

            form.Controls.Add(panelMid);
            form.Controls.Add(panelTop);
            form.Controls.Add(panelBot);

            btnSave.Click += (sender, e) =>
            {
                lblStatus.Text = "";
                string varName = txtVarName.Text.Trim();
                
                if (!Regex.IsMatch(varName, "^[a-zA-Z][a-zA-Z0-9_]*$"))
                {
                    MessageBox.Show("El nombre de variable no es válido.\nDebe usar letras, números o guiones bajos, sin espacios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string rawText = txtData.Text.TrimEnd();
                if (string.IsNullOrWhiteSpace(rawText))
                {
                    MessageBox.Show("No hay datos para guardar. Pega los números primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string[] lines = rawText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                List<double[]> matrixData = new List<double[]>();

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (string.IsNullOrEmpty(line)) continue;

                    string[] parts = line.Split('\t');
                    List<double> row = new List<double>();
                    
                    foreach (string part in parts)
                    {
                        string valStr = part.Trim().Replace(",", ""); 
                        if (valStr != "")
                        {
                            if (double.TryParse(valStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double val))
                            {
                                row.Add(val);
                            }
                            else
                            {
                                MessageBox.Show($"Encontré texto que no es número en la fila {i + 1}.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    if (row.Count > 0) matrixData.Add(row.ToArray());
                }

                if (matrixData.Count == 0) return;

                int cols = matrixData[0].Length;
                for (int i = 0; i < matrixData.Count; i++)
                {
                    if (matrixData[i].Length != cols)
                    {
                        MessageBox.Show($"La fila {i + 1} tiene {matrixData[i].Length} columnas en lugar de {cols}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                int rows = matrixData.Count;
                double[,] array2d = new double[rows, cols];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++) array2d[i, j] = matrixData[i][j];
                }

                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Archivos MATLAB|*.mat|Todos los archivos|*.*",
                    FileName = $"{varName}.mat"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var matrix = Matrix<double>.Build.DenseOfArray(array2d);
                        var matDict = new Dictionary<string, Matrix<double>> { { varName, matrix } };
                        MatlabWriter.Write(sfd.FileName, matDict);

                        lblStatus.Text = $"✅ Guardado con éxito: {varName}.mat ({rows}, {cols})";
                        lblStatus.ForeColor = Color.FromArgb(40, 167, 69); 
                        
                        txtData.Clear();
                        txtVarName.Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"No se pudo guardar:\n{ex.Message}", "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };

            Application.Run(form);
        }
    }
}