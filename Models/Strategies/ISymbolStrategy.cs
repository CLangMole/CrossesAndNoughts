﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CrossesAndNoughts.Models.SymbolsFactories;

namespace CrossesAndNoughts.Models.Strategies;

public interface ISymbolStrategy
{
    void DrawSymbol(Grid? field, int rowIndex, int columnIndex);
}

public class CrossesStrategy : ISymbolStrategy
{
    private readonly SymbolsFactory _crossesFactory = new CrossesFactory();

    public void DrawSymbol(Grid? field, int rowIndex, int columnIndex)
    {
        Image symbol = _crossesFactory.CreateSymbol();

        field?.Children.Add(symbol);

        Grid.SetRow(symbol, rowIndex);
        Grid.SetColumn(symbol, columnIndex);
    }
}

public class NoughtsStrategy : ISymbolStrategy
{
    private readonly SymbolsFactory _noughtsFactory = new NoughtsFactory();

    public void DrawSymbol(Grid? field, int rowIndex, int columnIndex)
    {
        Image symbol = _noughtsFactory.CreateSymbol();

        field?.Children.Add(symbol);

        Grid.SetRow(symbol, rowIndex);
        Grid.SetColumn(symbol, columnIndex);
    }
}
