using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//CURSO – LEGAJO – APELLIDO – NOMBRE

namespace ABMMascotas
{
    public partial class frmMascota : Form

    {
        accesoDatos oDB;
        private List<Mascota> lMascotas;

        public frmMascota()
        {
            InitializeComponent();
            oDB = new accesoDatos();
            lMascotas = new List<Mascota>();
        }

        private void frmMascota_Load(object sender, EventArgs e)
        {
            cargarCombo();
            cargarLista();
            habilitar(false);
        }
        private void cargarCombo() //metodo para cargar el combobox
        {

            DataTable tabla = oDB.consultarBD("SELECT * FROM Especies ORDER BY 2");
            cboEspecie.DataSource = tabla;
            cboEspecie.DisplayMember = "nombreEspecie";
            cboEspecie.ValueMember = "idEspecie";
            cboEspecie.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEspecie.SelectedIndex = 0;
        }

        private void cargarLista()//metodo para cargar la lista 
        {
            DataTable tabla = oDB.consultarBD("SELECT * FROM MASCOTAS");

            foreach (DataRow fila in tabla.Rows)
            {
                Mascota m = new Mascota();
                m.pCodigo = Convert.ToInt32(fila["codigo"].ToString());
                m.pNombre = Convert.ToString(fila["nombre"]);
                m.pEspecie = Convert.ToInt32(fila["especie"]);
                m.pSexo = Convert.ToInt32(fila["sexo"]);
                m.pFechaNacimiento = Convert.ToDateTime(fila["fechaNacimiento"]);
                lMascotas.Add(m);
                lstMascotas.Items.Add(m);
            }


        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Esta seguro que quiere salir ?"
                , "saliendo"
                , MessageBoxButtons.YesNo
                , (MessageBoxIcon.Question)
                , MessageBoxDefaultButton.Button2)
                == DialogResult.Yes)
                Close();

        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            habilitar(true);
            limpiar();
            txtCodigo.Focus();
        }
        private void limpiar()
        {
            txtCodigo.Text = "";
            txtNombre.Text = "";
            cboEspecie.SelectedIndex = -1;
            rbtHembra.Checked = false;
            rbtMacho.Checked=false;
            dtpFechaNacimiento.Value = DateTime.Today;
        }

        private void habilitar(bool v)
        {
            txtCodigo.Enabled = v;
            txtNombre.Enabled = v;
            cboEspecie.Enabled = v;
            rbtHembra.Enabled = v;
            rbtMacho.Enabled = v;
            dtpFechaNacimiento.Enabled = v;
            btnGrabar.Enabled= v;
            btnNuevo.Enabled = !v;
            btnSalir.Enabled = !v;
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (validarDatos()) // validar datos
            {
                //crear objeto
                Mascota m = new Mascota();
                m.pNombre = txtNombre.Text;
                m.pCodigo = int.Parse(txtCodigo.Text);
                m.pEspecie = Convert.ToInt32(cboEspecie.SelectedValue);
                if (rbtHembra.Checked)
                {
                    m.pSexo = 2;
                }
                else
                {
                    m.pSexo = 1;
                }
                m.pFechaNacimiento = dtpFechaNacimiento.Value;


                if (!existe(m))
                {
                    string insertSQL = "INSERT INTO MASCOTAS Values ( " +
                        m.pCodigo + "," + m.pNombre + "," + m.pEspecie + "," 
                        + m.pSexo + "," + m.pFechaNacimiento.ToString("yyyy/MM/dd") + "')"

                        if(oDB.actualizarBD(insertSQL)>0)
                    {
                        MessageBox.Show("se agrego con exito la nueva mascota");
                        cargarLista();
                    }
                    else
                    {
                        MessageBox.Show("la mascota ya existe");
                    }

                }
                habilitar(false);
            }
        }

        private bool existe(Mascota nueva)
        {
            for (int i = 0; i < lMascotas.Count; i++)
            {
                if (lMascotas[i].pCodigo == nueva.pCodigo) 
                return true;
            }
            return false;
        }
        private bool validarDatos()
        {
            bool valido = true;
            if(txtCodigo.Text == "")
            {
                MessageBox.Show("debe ingresar el codigo");
                txtCodigo.Focus();
                valido = false;
            }
             else if(txtNombre.Text == "")
            {
                MessageBox.Show("debe ingresar el nombre");
                txtNombre.Focus();
                valido = false;
            }
           else  if(cboEspecie.SelectedIndex== -1)
            {
                MessageBox.Show("debe ingresar la especie");
                cboEspecie.Focus();
                valido = false;

            }
          else if (!rbtHembra.Checked && !rbtMacho.Checked)
            {
                MessageBox.Show("debe seleccionar un sexo");
                rbtMacho.Focus();
                valido = false;
            }
            else if(DateTime.Today.Year - dtpFechaNacimiento.Value.Year > 10)
                {
                MessageBox.Show("la mascota debe tener menos de 10 años");
               dtpFechaNacimiento.Focus();
                valido = false;

            }

            return valido;
        }
    }
}
