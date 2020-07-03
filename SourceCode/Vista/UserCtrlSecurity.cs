using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SourceCode.Controlador;
using SourceCode.Modelo;

namespace SourceCode.Vista
{
    public partial class UserCtrlSecurity : UserControl
    {
        private Usuario user;
        
        private delegate void MyDelegate();
        static MyDelegate Actualizaciones;
        
        private List<Usuario> listU = new List<Usuario>();
        private List<Departamento> listD = new List<Departamento>();
        private List<Registro> listR = new List<Registro>();
        private List<UsuarioReducido> listUR = new List<UsuarioReducido>();
        
        public UserCtrlSecurity(Usuario usuario)
        {
            user = usuario;
            InitializeComponent();
        }

        private void UserCtrlSecurity_Load(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;

            Actualizaciones = ActualizarControles;
            Actualizaciones += ActualizarDataGrids;

            lblUser.Text = "Bienvenid@ " + user.nombre + " [Seguridad]";
            lblUser.TextAlign = ContentAlignment.BottomRight;
            lblUser.Font = new Font("Consolas", 14);
            
            title.Text = "Registro de temperaturas";
            title.Font = new Font("Consolas", 24);
            
            title2.Text = "Empleados actualmente dentro";
            title2.Font = new Font("Consolas", 24);
            
            btnAdd.Text = "AGREGAR";
            btnAdd.Font = new Font("Consolas", 10);

            btnReturn.Text = "REGRESAR";
            btnReturn.Font = new Font("Consolas", 10);

            numericUpDown1.Minimum = 0;

            radioButton1.Checked = true;

            foreach (Control ctrl in tableLayoutPanel2.Controls)
            {
                if (ctrl.Tag == "add")
                {
                    ctrl.Font = new Font("Consolas", 10);
                }
            }
            
            Actualizaciones.Invoke();
        }
        
        private void ActualizarControles()
        {
            listU = UsuarioDAO.getList();
            listD = DepartamentoDAO.getList();
            listR = RegistroDAO.getList();

            cmbId.DataSource = null;
            cmbId.DisplayMember = "idUsuario";
            cmbId.DataSource = listU;
        }

        private void ActualizarDataGrids()
        {
            listUR = UsuarioDAO.GetUsuariosEnEdificio();
            dataGridView1.DataSource = listUR;
        }

        private void btnReturn_Click_1(object sender, EventArgs e)
        {
            ((FormInterface)this.ParentForm).ChangeControl(new UserCtrLogin());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            bool entrada;
            if (radioButton1.Checked)
                entrada = true;
            else
                entrada = false; 
            try
            {
                RegistroDAO.AgregarRegistro(new Registro(entrada, dateTimePicker1.Value, 
                    (int) numericUpDown1.Value, cmbId.Text));
                MessageBox.Show("Registro agregado exitosamente");
                Actualizaciones.Invoke();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}