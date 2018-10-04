using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace EBC_InkDemo.ViewModels
{

    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

        }


    }   
}
