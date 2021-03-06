﻿using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Sistem_de_inventario
{
    public partial class fCuerina : Form
    {
        Validaciones _Validaciones;
        Administrador _Administrador;
        public fCuerina()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            _Validaciones = new Validaciones();
            _Administrador = new Administrador();
            if (FPrincipal.ModoEditar)
            {
                this.Text = "Editar";
                this.fCuerTxtNombre.Text = (FPrincipal.ItemSeleccionado.SubItems[1].Text).Trim();
                this.fCuerTxtAncho.Text = FPrincipal.ItemSeleccionado.SubItems[2].Text;
                this.fCuerTxtPrecio.Text = FPrincipal.ItemSeleccionado.SubItems[3].Text;
                this.fCuerTxtPorcentaje.Text = FPrincipal.ItemSeleccionado.SubItems[4].Text;
            }
        }
        private void FCuerBtnCancelar_Click(object sender, EventArgs e) { this.Close(); }
        private void FCuerBtnAceptar_Click(object sender, EventArgs e)
        {
            if (!FPrincipal.ModoEditar && this.ValidateChildren())
            {
                Cuerinas cuerina = new Cuerinas()
                {
                    Nombre = this.fCuerTxtNombre.Text,
                    Ancho = Convert.ToDecimal(this.fCuerTxtAncho.Text),
                    Precio = Convert.ToDecimal(this.fCuerTxtPrecio.Text),
                    PorcDeGanancia = Convert.ToInt32(this.fCuerTxtPorcentaje.Text),
                };
                _Administrador.GuardarNuevaCuerina(cuerina);
                this.Close();
            }
            else if (FPrincipal.ModoEditar && this.ValidateChildren())
            {
                Cuerinas cuerina = new Cuerinas()
                {
                    ID = Convert.ToInt32(FPrincipal.ItemSeleccionado.Text),
                    Nombre = this.fCuerTxtNombre.Text,
                    Ancho = Convert.ToDecimal(this.fCuerTxtAncho.Text),
                    Precio = Convert.ToDecimal(this.fCuerTxtPrecio.Text),
                    PorcDeGanancia = Convert.ToInt32(this.fCuerTxtPorcentaje.Text),
                };
                _Administrador.ActualizarCuerina(cuerina);
                this.Close();
            }
        }
        private void FCuerTxtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_Validaciones.EnterEsPresionado(e)) { this.GetNextControl(ActiveControl, true).Focus(); e.Handled = true; }
        }
        private void FCuerTxtAncho_KeyPress(object sender, KeyPressEventArgs e)
        {
            _Validaciones.SoloNumerosYComa(e);
            if (_Validaciones.EnterEsPresionado(e)) { this.GetNextControl(ActiveControl, true).Focus(); e.Handled = true; }
        }
        private void FCuerTxtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            _Validaciones.SoloNumerosYComa(e);
            if (_Validaciones.EnterEsPresionado(e)) { this.GetNextControl(ActiveControl, true).Focus(); e.Handled = true; }
        }
        private void FCuerTxtPorcentaje_KeyPress(object sender, KeyPressEventArgs e)
        {
            _Validaciones.SoloNumeros(e);
            if (_Validaciones.EnterEsPresionado(e)) { this.GetNextControl(ActiveControl, true).Focus(); e.Handled = true; }
        }
        private void FCuerTxtAncho_Validating(object sender, CancelEventArgs e)
        {
            if (!fCuerBtnCancelar.Focused)
            {
                if (!_Validaciones.EsDecimal(this.fCuerTxtAncho.Text)) { e.Cancel = true; }
            }
        }
        private void FCuerTxtPrecio_Validating(object sender, CancelEventArgs e)
        {
            if (!fCuerBtnCancelar.Focused)
            {
                if (!_Validaciones.EsDecimal(this.fCuerTxtPrecio.Text)) { e.Cancel = true; }
                else { this.fCuerLblPrecio.Text = "$" + ((Convert.ToDecimal(this.fCuerTxtPrecio.Text)) * FPrincipal.Dolar).ToString(); }
            }
        }
        private void FCuerTxtPorcentaje_Validating(object sender, CancelEventArgs e)
        {
            if (!fCuerBtnCancelar.Focused)
            {
                if (!_Validaciones.EsEntero(this.fCuerTxtPorcentaje.Text)) { e.Cancel = true; }
            }
        }
        private void FCuerTxtNombre_Validating(object sender, CancelEventArgs e)
        {
            if (!fCuerBtnCancelar.Focused)
            {
                if (_Validaciones.EsNullOTieneEspaciosEnBlanco(this.fCuerTxtNombre.Text)) { e.Cancel = true; }
            }
        }
    }
}
