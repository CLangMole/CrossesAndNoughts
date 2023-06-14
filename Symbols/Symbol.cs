using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CrossesAndNoughts.Symbols.Factories;

namespace CrossesAndNoughts.Symbols;

public abstract class Symbol
{
    public abstract void Accept(ISymbolVisitor symbolVisitor);

    public Symbol() { }
}

public class Cross : Symbol
{
    public override void Accept(ISymbolVisitor symbolVisitor)
    {
        symbolVisitor.Visit(this);
    }
}

public class Nought : Symbol
{
    public override void Accept(ISymbolVisitor symbolVisitor)
    {
        symbolVisitor.Visit(this);
    }
}
