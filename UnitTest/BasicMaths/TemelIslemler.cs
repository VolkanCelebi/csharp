using System;

namespace BasicMaths
{
    public class TemelIslemler
    {
        public double Topla(double num1, double num2)
        {
            return num1 + num2;
        }
        public double Cikart(double num1, double num2)
        {
            return num1 - num2;
        }
        public double Bol(double num1, double num2)
        {
            return num1 / num2;
        }
        public double Carp(double num1, double num2)
        {
            // işleç yanlış verildi.  
            return num1 * num2;
        }

        public bool Pozitifmi(int sayi)
        {
            if (sayi >= 0)
                return true;
            else
                return false;
        }
    }
}
