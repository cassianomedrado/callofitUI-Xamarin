using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CallofitMobileXamarin.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private Frame _frameClick;
        public ICommand FrameClickedCommand { get; private set; }


        public MainPageViewModel()
        {
            FrameClickedCommand = new Command<object>((sender) =>
            {
                var frame = sender as Frame;
                if (frame != null)
                {
                    if (_frameClick != null && _frameClick != frame)
                    {
                        _frameClick.BackgroundColor = Color.White;
                        _frameClick.ScaleTo(1, 100, Easing.CubicIn);
                    }

                    frame.BackgroundColor = Color.DarkGray;
                    frame.ScaleTo(0.9, 100, Easing.CubicOut);
                    _frameClick = frame;

                    Device.StartTimer(TimeSpan.FromMilliseconds(50), () => // tempo em milissegundos
                    {
                        frame.BackgroundColor = Color.White;
                        frame.ScaleTo(1, 100, Easing.CubicIn);
                        _frameClick = null;
                        return false;
                    });             
                }
            });

        }

    }
}
