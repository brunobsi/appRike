﻿namespace appRike.Funções
{
    partial class FuncaoAgenda
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FuncaoAgenda));
            this.cbbAluno = new System.Windows.Forms.ComboBox();
            this.btnAlunoAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAgendar = new System.Windows.Forms.Button();
            this.lbMensagem = new System.Windows.Forms.Label();
            this.cbbHorario = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnHorarioAdd = new System.Windows.Forms.Button();
            this.cbbComputador = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnComputadorAdd = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbbAluno
            // 
            this.cbbAluno.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbAluno.FormattingEnabled = true;
            this.cbbAluno.Location = new System.Drawing.Point(6, 35);
            this.cbbAluno.Name = "cbbAluno";
            this.cbbAluno.Size = new System.Drawing.Size(346, 21);
            this.cbbAluno.TabIndex = 7;
            this.cbbAluno.SelectedIndexChanged += new System.EventHandler(this.cbbAluno_SelectedIndexChanged);
            // 
            // btnAlunoAdd
            // 
            this.btnAlunoAdd.Location = new System.Drawing.Point(372, 35);
            this.btnAlunoAdd.Name = "btnAlunoAdd";
            this.btnAlunoAdd.Size = new System.Drawing.Size(25, 23);
            this.btnAlunoAdd.TabIndex = 8;
            this.btnAlunoAdd.Text = "+";
            this.btnAlunoAdd.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Aluno";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnAgendar);
            this.groupBox2.Controls.Add(this.lbMensagem);
            this.groupBox2.Controls.Add(this.cbbHorario);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btnHorarioAdd);
            this.groupBox2.Controls.Add(this.cbbComputador);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btnComputadorAdd);
            this.groupBox2.Controls.Add(this.cbbAluno);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btnAlunoAdd);
            this.groupBox2.Location = new System.Drawing.Point(21, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(422, 230);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            // 
            // btnAgendar
            // 
            this.btnAgendar.Image = global::appRike.Properties.Resources.agendar;
            this.btnAgendar.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnAgendar.Location = new System.Drawing.Point(299, 194);
            this.btnAgendar.Name = "btnAgendar";
            this.btnAgendar.Size = new System.Drawing.Size(98, 25);
            this.btnAgendar.TabIndex = 18;
            this.btnAgendar.Text = "   Agendar";
            this.btnAgendar.UseVisualStyleBackColor = true;
            this.btnAgendar.Click += new System.EventHandler(this.btnAgendar_Click);
            // 
            // lbMensagem
            // 
            this.lbMensagem.AutoSize = true;
            this.lbMensagem.Location = new System.Drawing.Point(9, 200);
            this.lbMensagem.Name = "lbMensagem";
            this.lbMensagem.Size = new System.Drawing.Size(67, 13);
            this.lbMensagem.TabIndex = 17;
            this.lbMensagem.Text = "lbMensagem";
            this.lbMensagem.Visible = false;
            // 
            // cbbHorario
            // 
            this.cbbHorario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbHorario.FormattingEnabled = true;
            this.cbbHorario.Location = new System.Drawing.Point(9, 155);
            this.cbbHorario.Name = "cbbHorario";
            this.cbbHorario.Size = new System.Drawing.Size(343, 21);
            this.cbbHorario.TabIndex = 13;
            this.cbbHorario.SelectedIndexChanged += new System.EventHandler(this.cbbHorario_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Horario";
            // 
            // btnHorarioAdd
            // 
            this.btnHorarioAdd.Location = new System.Drawing.Point(372, 155);
            this.btnHorarioAdd.Name = "btnHorarioAdd";
            this.btnHorarioAdd.Size = new System.Drawing.Size(25, 23);
            this.btnHorarioAdd.TabIndex = 14;
            this.btnHorarioAdd.Text = "+";
            this.btnHorarioAdd.UseVisualStyleBackColor = true;
            // 
            // cbbComputador
            // 
            this.cbbComputador.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbComputador.FormattingEnabled = true;
            this.cbbComputador.Location = new System.Drawing.Point(9, 96);
            this.cbbComputador.Name = "cbbComputador";
            this.cbbComputador.Size = new System.Drawing.Size(343, 21);
            this.cbbComputador.TabIndex = 10;
            this.cbbComputador.SelectedIndexChanged += new System.EventHandler(this.cbbComputador_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Computador";
            // 
            // btnComputadorAdd
            // 
            this.btnComputadorAdd.Location = new System.Drawing.Point(372, 96);
            this.btnComputadorAdd.Name = "btnComputadorAdd";
            this.btnComputadorAdd.Size = new System.Drawing.Size(25, 23);
            this.btnComputadorAdd.TabIndex = 11;
            this.btnComputadorAdd.Text = "+";
            this.btnComputadorAdd.UseVisualStyleBackColor = true;
            // 
            // FuncaoAgenda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 250);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FuncaoAgenda";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Agenda";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbbAluno;
        private System.Windows.Forms.Button btnAlunoAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbMensagem;
        private System.Windows.Forms.ComboBox cbbHorario;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnHorarioAdd;
        private System.Windows.Forms.ComboBox cbbComputador;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnComputadorAdd;
        private System.Windows.Forms.Button btnAgendar;
    }
}