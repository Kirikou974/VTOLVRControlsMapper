﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using VTOLVRControlsMapper;
using VTOLVRControlsMapper.Core;
using VTOLVRControlsMapperUI.BindingItem;
using VTOLVRControlsMapperUI.GridItem;

namespace VTOLVRControlsMapperUI.BindingWindows
{
    /// <summary>
    /// Interaction logic for ThrottleBinding.xaml
    /// </summary>
    public partial class ThrottleBinding : Window
    {
        public List<IBindingItem> BindingItems;
        public ThrottleBinding(Window sender, ControlMapping mapping)
        {
            InitializeComponent();
            Owner = sender;
            Title = mapping.GameControlName;
            LoadDevicesTab(mapping);
        }
        private void LoadDevicesTab(ControlMapping mapping)
        {
            Dictionary<Guid, string> availableDevices = ControlsHelper.GetAvailableDevices();
            BindingItems = new List<IBindingItem>();
            foreach (Guid deviceID in availableDevices.Keys)
            {
                DeviceItem device = new DeviceItem(deviceID, availableDevices[deviceID]);
                ThrottleBindingItem throttleBindingItem = new ThrottleBindingItem(device);
                if (mapping.GameActions != null &&
                    mapping.GameActions.Find(g =>
                        g != null &&
                        g is ThrottleAction &&
                        g.ControllerInstanceGuid == deviceID) is ThrottleAction throttleAction)
                {
                    throttleBindingItem.MappingAction = throttleAction;
                }
                BindingItems.Add(throttleBindingItem);
            }
            DevicesTab.ItemsSource = BindingItems;
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            BaseItem item = ((FrameworkElement)sender).DataContext as BaseItem;
            Helper.EditBinding(DevicesTab, item, WaitMessage, WaitMessageRectangle);
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            BaseItem item = ((FrameworkElement)sender).DataContext as BaseItem;
            Helper.ClearBinding(DevicesTab, item);
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem is MappingRange mappingRange &&
                ((FrameworkElement)sender).DataContext is AxisItem item)
            {
                item.Range = mappingRange;
            }
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            HandleCheckBox(sender);
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            HandleCheckBox(sender);
        }
        private void HandleCheckBox(object sender)
        {
            if (((FrameworkElement)sender).DataContext is AxisItem item && sender is CheckBox checkBox)
            {
                item.Invert = checkBox.IsChecked.Value;
            }
        }
    }
}
