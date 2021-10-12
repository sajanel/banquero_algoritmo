using System;
using System.Data;
using System.Windows.Forms;

namespace banquero_algoritmo
{
    public partial class Form1 : Form
    {
        private int[,] necesarios;
        private int[,] asignados;
        private int[,] maximos;
        private int[,] disponible;
        private int numeroProcesos, numeroRecursos;
        String nombreValor;

        private void bntAceptar_Click(object sender, EventArgs e)
        {

            necesarios = new int [numeroProcesos,numeroRecursos];
            asignados = new int[numeroProcesos, numeroRecursos];
            maximos = new int[numeroProcesos, numeroRecursos];
            disponible = new int[1, numeroRecursos];
            
            // Informacion para asignados
            for (int i = 0; i < numeroProcesos; i++)
            {
                for (int j = 0; j < numeroRecursos; j++)
                {
                    //Rows[fila].Cells[col].Value.ToString();
                    nombreValor = dataGridView1.Rows[i].Cells[j].Value.ToString();           
                    asignados[i, j] = int.Parse(nombreValor);
                }
            }

            // Informacion para Maximos
            for (int i = 0; i < numeroProcesos; i++)
            {
                for (int j = 0; j < numeroRecursos; j++)
                {
                    nombreValor = dataGridView2.Rows[i].Cells[j].Value.ToString();
                    maximos[i, j] = int.Parse(nombreValor);
                }
            }

            // Ultima tabla 
            for (int j = 0; j < this.numeroRecursos; j++)
            {
                nombreValor = dataGridView3.Rows[0].Cells[j].Value.ToString();
                disponible[0, j] = int.Parse(nombreValor);  //matriz de disponibles
            }

            esSeguro();
            bntAceptar.Enabled = false;
        }

        private int[,] calculoNecesarios()
        {
            for (int i = 0; i < numeroProcesos; i++)
            {
                for (int j = 0; j < numeroRecursos; j++) //calculando matriz de necesarios
                {
                    necesarios[i,j] = maximos[i,j] - asignados[i,j];
                }
            }

            return necesarios;
        }

        private Boolean chequear(int i)
        {
            //chequeando si todos los recursos para el proceso pueden ser asignados
            for (int j = 0; j < numeroRecursos; j++)
            {
                if (disponible[0,j] < necesarios[i,j])
                {
                    return false;
                }
            }

            return true;
        }

        public Form1()
        {
            InitializeComponent();
        }

        // Crear las tablas 
        private void button1_Click(object sender, EventArgs e)
        {
             numeroProcesos = comboBox1.SelectedIndex + 1;
             numeroRecursos = comboBox2.SelectedIndex + 1;
            limpiar();

            dataGridView1.ColumnCount = numeroRecursos;
            dataGridView1.RowCount = numeroProcesos;
          
            dataGridView2.ColumnCount = numeroRecursos;
            dataGridView2.RowCount = numeroProcesos;

            dataGridView3.ColumnCount = numeroRecursos;
            dataGridView3.RowCount = 1;

            bntAceptar.Enabled = true;
            listBox1.Items.Clear();
        }
        public void limpiar() 
        {
            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();
            dataGridView3.ClearSelection();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
           
      
        }

        public void esSeguro()
        {
           
            calculoNecesarios();
            Boolean[] done = new Boolean[numeroProcesos];
            int j = 0;

            while (j < numeroProcesos)
            {  //hasta que todos los procesos se asignen
                Boolean asignado = false;
                for (int i = 0; i < numeroProcesos; i++)
                {
                    if (!done[i] && chequear(i))
                    {  //intentando asignar
                        for (int k = 0; k < numeroRecursos; k++)
                        {
                           disponible[0,k] = disponible[0,k] - necesarios[i,k] + maximos[i,k];
                        }
                        listBox1.Items.Add("Proceso asignado :" + i);
                        asignado = done[i] = true;
                        j++;
                    }
                }
                if (!asignado)
                {
                    break;  //si no esta asignado
                }
            }
            if (j == numeroProcesos) //si todos los procesos estan asignados
            {
                MessageBox.Show("Asignado de forma segura");
            }
            else
            {
                MessageBox.Show("Todos los procesos se pueden asignar de forma segura");
            }

        }
        }
}
