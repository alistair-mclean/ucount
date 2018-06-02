from kivy.uix.colorpicker import ColorPicker
from kivy.uix.widget import Widget

parent = Widget()
clr_picker = ColorPicker()
parent.add_widget(clr_picker)
Widget 
# To monitor changes, we can bind to color property changes
def on_color(instance, value):
    print ("RGBA = ", str(value))  #  or instance.color
    print ("HSV = ", str(instance.hsv))
    print ("HEX = ", str(instance.hex_color))

clr_picker.bind(color=on_color)