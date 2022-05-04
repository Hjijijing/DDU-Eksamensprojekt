using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomUtil
{
    static int[] shellOrder = new int[] { 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 5, 5, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 6, 6, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7 };
    //static string[] atomNames = new string[] { "None", "Hydrogen", "Helium", "Lithium", "Beryllium", "Boron", "Carbon", "Nitrogen", "Oxygen", "Fluorine", "Neon", "Sodium", "Magnesium", "Aluminum", "Silicon", "Phosphorus", "Sulfur", "Chlorine", "Argon", "Potassium", "Calcium", "Scandium", "Titanium", "Vanadium", "Chromium", "Manganese", "Iron", "Cobalt", "Nickel", "Copper", "Zinc", "Gallium", "Germanium", "Arsenic", "Selenium", "Bromine", "Krypton", "Rubidium", "Strontium", "Yttrium", "Zirconium", "Niobium", "Molybdenum", "Technetium", "Ruthenium", "Rhodium", "Palladium", "Silver", "Cadmium", "Indium", "Tin", "Antimony", "Tellurium", "Iodine", "Xenon", "Cesium", "Barium", "Lanthanum", "Cerium", "Praseodymium", "Neodymium", "Promethium", "Samarium", "Europium", "Gadolinium", "Terbium", "Dysprosium", "Holmium", "Erbium", "Thulium", "Ytterbium", "Lutetium", "Hafnium", "Tantalum", "Wolfram", "Rhenium", "Osmium", "Iridium", "Platinum", "Gold", "Mercury", "Thallium", "Lead", "Bismuth", "Polonium", "Astatine", "Radon", "Francium", "Radium", "Actinium", "Thorium", "Protactinium", "Uranium", "Neptunium", "Plutonium", "Americium", "Curium", "Berkelium", "Californium", "Einsteinium", "Fermium", "Mendelevium", "Nobelium", "Lawrencium", "Rutherfordium", "Dubnium", "Seaborgium", "Bohrium", "Hassium", "Meitnerium", "Darmstadtium ", "Roentgenium ", "Copernicium ", "Nihonium", "Flerovium", "Moscovium", "Livermorium", "Tennessine", "Oganesson" };
    //static string[] atomSymbols = new string[] { "n", "H", "He", "Li", "Be", "B", "C", "N", "O", "F", "Ne", "Na", "Mg", "Al", "Si", "P", "S", "Cl", "Ar", "K", "Ca", "Sc", "Ti", "V", "Cr", "Mn", "Fe", "Co", "Ni", "Cu", "Zn", "Ga", "Ge", "As", "Se", "Br", "Kr", "Rb", "Sr", "Y", "Zr", "Nb", "Mo", "Tc", "Ru", "Rh", "Pd", "Ag", "Cd", "In", "Sn", "Sb", "Te", "I", "Xe", "Cs", "Ba", "La", "Ce", "Pr", "Nd", "Pm", "Sm", "Eu", "Gd", "Tb", "Dy", "Ho", "Er", "Tm", "Yb", "Lu", "Hf", "Ta", "W", "Re", "Os", "Ir", "Pt", "Au", "Hg", "Tl", "Pb", "Bi", "Po", "At", "Rn", "Fr", "Ra", "Ac", "Th", "Pa", "U", "Np", "Pu", "Am", "Cm", "Bk", "Cf", "Es", "Fm", "Md", "No", "Lr", "Rf", "Db", "Sg", "Bh", "Hs", "Mt", "Ds ", "Rg ", "Cn ", "Nh", "Fl", "Mc", "Lv", "Ts", "Og" };

    static float protonMass = 1.0073f;
    static float neutronMass = 1.0087f;
    static float electronMass = 0.00055f;

    public const float HIGHESTATOM = 118; 

    public static int getOuterShell(int electronNumber)
    {
        if (electronNumber < 1 || electronNumber > shellOrder.Length)
            return 0;
        return shellOrder[electronNumber - 1];
    }


    public static int[] getShellConfiguration(int electronAmount)
    {
        int[] result = new int[] { 0, 0, 0, 0, 0, 0, 0 };
        for(int i = 0; i < electronAmount; i++)
        {
            result[getOuterShell(i + 1)-1]++;
        }

        return result;
    }


    public static string getAtomName(int protonAmount)
    {
        if (protonAmount == 0) return "None";
        if (protonAmount > HIGHESTATOM) return "N/A";
        return IsotopeManager.isotopeManager.getElement(protonAmount).elementName;
    }

    public static string getAtomSymbol(int protonAmount)
    {
        if (protonAmount == 0) return "n";
        if (protonAmount > HIGHESTATOM) return "N/A";
        return IsotopeManager.isotopeManager.getElement(protonAmount).elementSymbol;
    }

    public static float getMass(int protons, int neutrons, int electrons)
    {
        return Mathf.Round(1000*(protons * protonMass + neutrons * neutronMass + electrons * electronMass))/1000;
    }


    
    


}
