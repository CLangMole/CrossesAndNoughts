using System;
using CrossesAndNoughts.Models.DataBase;

namespace CrossesAndNoughts.ViewModel;

public partial class AppViewModel
{
    private class UiRefresher
    {
        private static AppViewModel? _viewModel;

        internal UiRefresher(AppViewModel appViewModel)
        {
            _viewModel = appViewModel;
        }

        internal static void RefreshRecordsList()
        {
            if (_viewModel is null)
            {
                throw new NullReferenceException(nameof(_viewModel));
            }

            using var records = new UserRecordsProxy();

            _viewModel.Records = records.GetRecords();
        }

        internal static void RefreshPoints(int points)
        {
            if (_viewModel is null)
            {
                throw new NullReferenceException(nameof(_viewModel));
            }

            _viewModel.Points = points.ToString();
        }

        internal static void RefreshDifficultyProperties(System.Windows.Media.Brush brush, string name)
        {
            if (_viewModel is null)
            {
                throw new NullReferenceException(nameof(_viewModel));
            }

            _viewModel.DifficultyColor = brush;
            _viewModel.DifficultyName = name;
        }
    }
}