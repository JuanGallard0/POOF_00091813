using System;
using System.Drawing;
using System.Windows.Forms;
using SourceCode.Controlador;
using SourceCode.Modelo;
using SourceCode.Patrón;
using SourceCode.Patrón.Estrategia;
using SourceCode.Properties;

namespace SourceCode.Vista
{
    public partial class UserCtrLogin : UserControl
    {
        public UserCtrLogin()
        {
            InitializeComponent();
        }

        private void UserCtrLogin_Load(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;

            title.Text = "Iniciar Sesión";
            title.TextAlign = ContentAlignment.TopCenter;
            title.Font = new Font("Consolas", 24);
            
            label1.Text = "Carné:";
            label1.Font = new Font("Consolas", 14);

            label2.Text = "Contraseña:";
            label2.Font = new Font("Consolas", 14);

            btnLogin.Text = "ENTRAR";
            btnLogin.Font = new Font("Consolas", 14);

            pictureBox1.Image = Resources.Virus;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            ProxyAcceso.ISujeto proxy = new ProxyAcceso.ProxySeguro();
            if (proxy.PeticionAcceso(txtUser.Text, txtPwd.Text))
            {
                Usuario u = new Usuario();
                u = UsuarioDAO.GetSingleUsuario(txtUser.Text);

                string nombreDepartamento = DepartamentoDAO.GetNombreDepartamento(u.idDepartamento);

                IDepartamento miDepartamento;
                
                miDepartamento = new CAdministracion();
                if (miDepartamento.PerteneceADepartamento(nombreDepartamento))
                {
                    ((FormInterface)this.ParentForm).ChangeControl(new UserCtrlAdmin(u));
                }
                
                miDepartamento = new CVigilancia();
                if (miDepartamento.PerteneceADepartamento(nombreDepartamento))
                {
                    ((FormInterface)this.ParentForm).ChangeControl(new UserCtrlSecurity(u));
                }
                
                miDepartamento = new CPersonal();
                if (miDepartamento.PerteneceADepartamento(nombreDepartamento))
                {
                    ((FormInterface)this.ParentForm).ChangeControl(new UserCtrPersonel(u));
                }
            }
            else
            {
                MessageBox.Show("Carné o contraseña inválidos");
            }
        }
    }
}