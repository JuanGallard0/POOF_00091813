using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SourceCode.Controlador;
using SourceCode.Modelo;

namespace SourceCode.Vista
{
    public partial class UserCtrlAdmin : UserControl
    {
        private Usuario user;
        
        private delegate void MyDelegate();
        static MyDelegate Actualizaciones;
        
        private List<Usuario> listU = new List<Usuario>();
        private List<Departamento> listD = new List<Departamento>();
        private List<Registro> listR = new List<Registro>();
        private List<UsuarioReducido> listUR = new List<UsuarioReducido>();
        
        public UserCtrlAdmin(Usuario usuario)
        {
            user = usuario;
            InitializeComponent();
        }

        private void UserCtrlAdmin_Load(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;
            
            Actualizaciones = ActualizarControles;
            Actualizaciones += ActualizarDataGrids;
            
            tabPage1.Text = "Mantenimiento de usuarios";

            tabPage2.Text = "Resumenes";

            lblUser.Text = "Bienvenid@ " + user.nombre + " [Administrador]";
            lblUser.TextAlign = ContentAlignment.BottomRight;
            lblUser.Font = new Font("Consolas", 10);
            
            lvlTop.Font = new Font("Consolas", 14);
            
            lblfreq.Font = new Font("Consolas", 14);

            title.Text = "Agregar usuario";
            title.Font = new Font("Consolas", 24);

            title2.Text = "Eliminar usuario";
            title2.Font = new Font("Consolas", 24);

            titleReg.Text = "Registro General";
            titleReg.Font = new Font("Consolas", 14);

            titleTop.Text = "Departamento más concurrido:";
            titleTop.Font = new Font("Consolas", 14);
            
            titlebuild.Text = "Empleados dentro de la empresa";
            titlebuild.Font = new Font("Consolas", 24);

            btnReturn.Text = "REGRESAR";
            btnReturn.Font = new Font("Consolas", 10);
            
            btnAdd.Text = "AGREGAR";
            btnAdd.Font = new Font("Consolas", 10);

            btnDel.Text = "ELIMINAR";
            btnDel.Font = new Font("Consolas", 10);

            btnClear.Text = "Limpiar";
            btnClear.Font = new Font("Consolas", 6);
            
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

            cmbD.DataSource = null;
            cmbD.DisplayMember = "nombre";
            cmbD.ValueMember = "idDepartamento";
            cmbD.DataSource = listD;

            cmbId.DataSource = null;
            cmbId.DisplayMember = "idUsuario";
            cmbId.DataSource = listU;
        }

        private void ActualizarDataGrids()
        {
            listR = RegistroDAO.getList();
            listUR = UsuarioDAO.GetUsuariosEnEdificio();
            Frequencia f = DepartamentoDAO.BuscarDepartamentoConcurrido();

            dataRegist.DataSource = listR;
            dataBuilding.DataSource = listUR;
            lblfreq.Text = f.frecuencia.ToString();
            lvlTop.Text = f.nombre;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Usuario u = new Usuario(txtID.Text, txtPwd.Text, txtName.Text, txtSurn.Text, 
                Convert.ToInt32(txtDui.Text), dateTimePicker1.Value, (int) cmbD.SelectedValue);
            try
            {
                UsuarioDAO.AgregarUsuario(u);
                MessageBox.Show("Usuario agregado existosamente");
                btnClear_Click(sender, e);
                Actualizaciones.Invoke();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            TextBox txt = new TextBox();
            foreach (Control ctrl in tableLayoutPanel2.Controls)
            {
                if (ctrl is TextBox)
                {
                    txt = (TextBox) ctrl;
                    txt.Clear();
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioDAO.borrarUsuario(cmbId.Text);
                MessageBox.Show("Usuario borrado exitosamente");
                Actualizaciones.Invoke();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            ((FormInterface)this.ParentForm).ChangeControl(new UserCtrLogin());
        }
    }
}