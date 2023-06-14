using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossesAndNoughts.Symbols;

namespace CrossesAndNoughts;

public interface ISymbolVisitor
{
    void Visit(Symbol symbol);
    void Visit(Cross cross);
    void Visit(Nought nought);
}
