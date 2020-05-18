namespace Cliente
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.txtServidor = new System.Windows.Forms.MaskedTextBox();
            this.btnConectar = new System.Windows.Forms.Button();
            this.rxtMensajes = new System.Windows.Forms.RichTextBox();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lstClientes = new System.Windows.Forms.ListBox();
            this.btnA1 = new System.Windows.Forms.Button();
            this.btnA2 = new System.Windows.Forms.Button();
            this.btnA3 = new System.Windows.Forms.Button();
            this.btnA4 = new System.Windows.Forms.Button();
            this.btnA6 = new System.Windows.Forms.Button();
            this.btnA7 = new System.Windows.Forms.Button();
            this.btnA8 = new System.Windows.Forms.Button();
            this.btnA9 = new System.Windows.Forms.Button();
            this.btnA5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(101, 58);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(153, 20);
            this.txtNombre.TabIndex = 0;
            // 
            // txtServidor
            // 
            this.txtServidor.Location = new System.Drawing.Point(12, 16);
            this.txtServidor.Name = "txtServidor";
            this.txtServidor.Size = new System.Drawing.Size(64, 20);
            this.txtServidor.TabIndex = 1;
            this.txtServidor.Text = "127.0.0.1";
            this.txtServidor.Visible = false;
            // 
            // btnConectar
            // 
            this.btnConectar.Location = new System.Drawing.Point(278, 56);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(116, 22);
            this.btnConectar.TabIndex = 2;
            this.btnConectar.Text = "Ingresar al Chat";
            this.btnConectar.UseVisualStyleBackColor = true;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // rxtMensajes
            // 
            this.rxtMensajes.Location = new System.Drawing.Point(82, 7);
            this.rxtMensajes.Name = "rxtMensajes";
            this.rxtMensajes.Size = new System.Drawing.Size(222, 13);
            this.rxtMensajes.TabIndex = 3;
            this.rxtMensajes.Text = "";
            this.rxtMensajes.Visible = false;
            // 
            // btnEnviar
            // 
            this.btnEnviar.Location = new System.Drawing.Point(327, 125);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(67, 25);
            this.btnEnviar.TabIndex = 5;
            this.btnEnviar.Text = "Enviar!";
            this.btnEnviar.UseVisualStyleBackColor = true;
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(327, 170);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(64, 21);
            this.btnSalir.TabIndex = 6;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(161, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Opciones para Conexion";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Nombre Usuario:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(98, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Clientes Conectados";
            // 
            // lstClientes
            // 
            this.lstClientes.FormattingEnabled = true;
            this.lstClientes.Location = new System.Drawing.Point(42, 125);
            this.lstClientes.Name = "lstClientes";
            this.lstClientes.Size = new System.Drawing.Size(228, 186);
            this.lstClientes.TabIndex = 11;
            // 
            // btnA1
            // 
            this.btnA1.Location = new System.Drawing.Point(34, 125);
            this.btnA1.Name = "btnA1";
            this.btnA1.Size = new System.Drawing.Size(64, 55);
            this.btnA1.TabIndex = 12;
            this.btnA1.UseVisualStyleBackColor = true;
            this.btnA1.Click += new System.EventHandler(this.btnA1_Click);
            // 
            // btnA2
            // 
            this.btnA2.Location = new System.Drawing.Point(123, 125);
            this.btnA2.Name = "btnA2";
            this.btnA2.Size = new System.Drawing.Size(64, 55);
            this.btnA2.TabIndex = 13;
            this.btnA2.UseVisualStyleBackColor = true;
            this.btnA2.Click += new System.EventHandler(this.btnA2_Click);
            // 
            // btnA3
            // 
            this.btnA3.Location = new System.Drawing.Point(206, 125);
            this.btnA3.Name = "btnA3";
            this.btnA3.Size = new System.Drawing.Size(64, 55);
            this.btnA3.TabIndex = 14;
            this.btnA3.UseVisualStyleBackColor = true;
            this.btnA3.Click += new System.EventHandler(this.btnA3_Click);
            // 
            // btnA4
            // 
            this.btnA4.Location = new System.Drawing.Point(34, 195);
            this.btnA4.Name = "btnA4";
            this.btnA4.Size = new System.Drawing.Size(64, 55);
            this.btnA4.TabIndex = 15;
            this.btnA4.UseVisualStyleBackColor = true;
            this.btnA4.Click += new System.EventHandler(this.btnA4_Click);
            // 
            // btnA6
            // 
            this.btnA6.Location = new System.Drawing.Point(206, 195);
            this.btnA6.Name = "btnA6";
            this.btnA6.Size = new System.Drawing.Size(64, 55);
            this.btnA6.TabIndex = 17;
            this.btnA6.UseVisualStyleBackColor = true;
            this.btnA6.Click += new System.EventHandler(this.btnA6_Click);
            // 
            // btnA7
            // 
            this.btnA7.Location = new System.Drawing.Point(34, 270);
            this.btnA7.Name = "btnA7";
            this.btnA7.Size = new System.Drawing.Size(64, 55);
            this.btnA7.TabIndex = 18;
            this.btnA7.UseVisualStyleBackColor = true;
            this.btnA7.Click += new System.EventHandler(this.btnA7_Click);
            // 
            // btnA8
            // 
            this.btnA8.Location = new System.Drawing.Point(123, 270);
            this.btnA8.Name = "btnA8";
            this.btnA8.Size = new System.Drawing.Size(64, 55);
            this.btnA8.TabIndex = 19;
            this.btnA8.UseVisualStyleBackColor = true;
            this.btnA8.Click += new System.EventHandler(this.btnA8_Click);
            // 
            // btnA9
            // 
            this.btnA9.Location = new System.Drawing.Point(206, 270);
            this.btnA9.Name = "btnA9";
            this.btnA9.Size = new System.Drawing.Size(64, 55);
            this.btnA9.TabIndex = 20;
            this.btnA9.UseVisualStyleBackColor = true;
            this.btnA9.Click += new System.EventHandler(this.btnA9_Click);
            // 
            // btnA5
            // 
            this.btnA5.Location = new System.Drawing.Point(123, 195);
            this.btnA5.Name = "btnA5";
            this.btnA5.Size = new System.Drawing.Size(64, 55);
            this.btnA5.TabIndex = 21;
            this.btnA5.UseVisualStyleBackColor = true;
            this.btnA5.Click += new System.EventHandler(this.btnA5_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 346);
            this.Controls.Add(this.btnA5);
            this.Controls.Add(this.btnA9);
            this.Controls.Add(this.btnA8);
            this.Controls.Add(this.btnA7);
            this.Controls.Add(this.btnA6);
            this.Controls.Add(this.btnA4);
            this.Controls.Add(this.btnA3);
            this.Controls.Add(this.btnA2);
            this.Controls.Add(this.btnA1);
            this.Controls.Add(this.lstClientes);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnEnviar);
            this.Controls.Add(this.rxtMensajes);
            this.Controls.Add(this.btnConectar);
            this.Controls.Add(this.txtServidor);
            this.Controls.Add(this.txtNombre);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.MaskedTextBox txtServidor;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.RichTextBox rxtMensajes;
        private System.Windows.Forms.Button btnEnviar;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lstClientes;
        private System.Windows.Forms.Button btnA1;
        private System.Windows.Forms.Button btnA2;
        private System.Windows.Forms.Button btnA3;
        private System.Windows.Forms.Button btnA4;
        private System.Windows.Forms.Button btnA6;
        private System.Windows.Forms.Button btnA7;
        private System.Windows.Forms.Button btnA8;
        private System.Windows.Forms.Button btnA9;
        private System.Windows.Forms.Button btnA5;
    }
}

