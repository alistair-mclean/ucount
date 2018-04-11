# The module to control the file tab for BIOCOUNTER
# Author - Alistair McLean
# ----------------------------------------
# This tab handles the functionality to allow the 
# user to control which files they wish to analyze

# TODO  
# - Disable Analyze until all settings are set
# - Improve the color selection 
#  --> Maybe a zoomed in view where the mouse is hovering over
# - 

from kivy.app import App
from kivy.uix.boxlayout import BoxLayout
from kivy.uix.floatlayout import FloatLayout
from kivy.factory import Factory
from kivy.adapters.listadapter import ListAdapter
from kivy.uix.listview import ListItemButton	
from kivy.uix.button import Button
from kivy.uix.label import Label
from kivy.properties import ObjectProperty
from kivy.uix.popup import Popup
from kivy.config import Config
import numpy as np
import os
from src.bioAnalyzer import bioAnalyzer
import cv2
import ntpath

# Window configurations
Config.set('kivy', 'window_icon', 'src/res/Analyze.png')
Config.set('graphics', 'width', '1000')
Config.set('graphics', 'height', '500')
Config.set('graphics', 'resizable', False)

# LEFTOFF NOTES
# - Trying to make the buttons change color from hsv to 
# - Need to fix the color mode picker, or at least how the analyzer is interpretting the values

class LoadDialog(FloatLayout):
	load = ObjectProperty(None)
	cancel = ObjectProperty(None)

class Root(FloatLayout):
	loadfile = ObjectProperty(None)
	loadString = "Select the file you wish to analyze."
	

	def startup(self):
		pass
		self.hasLowColor = False # Figure out better logic for this
		self.hasHighColor = False # Figure out better logic for this
	
	def dismiss_popup(self):
		self._popup.dismiss()

	def show_load(self):
		content = LoadDialog(load=self.load, 
							cancel=self.dismiss_popup)
		self._popup = Popup(title="Load file", content=content,
							size_hint=(0.9,0.9))
		self._popup.open()

	def load(self,filepath, filename):
		self.filepath = filepath
		self.filename = filename
		if filename:
			fname = ntpath.basename(filename[0])
		else:
			return

		# TODO - add a check to make sure that it is an image extension!!!!
		self.loadString = str('{} ready for analyzing'.format(fname))
		self.ids.load_text.text = self.loadString
		self.ids.load_image.source = filename[0]

		# ENABLE Analyzer
		self.ids.analyze_tab.disabled = False
		self.dismiss_popup()

	def analyze(self):
		self.analyzer = bioAnalyzer(self.filename[0])
		fname = ntpath.basename(self.filename[0])
		# Get the values from the UI
		# Name
		organismName = self.ids.name_input.text
		# TODO - VALIDATE
		# Size
		sizeRange = [int(self.ids.low_size_input.text), int(self.ids.high_size_input.text)] 
		# TODO - VALIDATE
		
		# Color Range
		colorRange = [self.lowColor, self.highColor]
		# TODO - VALIDATE
		self.loadString = str('Analyzing {} '.format(fname))
		self.ids.load_text.text = self.loadString
		self.analyzer.analyzeOrganism(organismName, sizeRange, colorRange)
		print(colorRange) # debug
		self.loadString = str('Finished analyzing {}\n'.format(fname) +
							  'Results saved in  /results\n' + # Maybe add the ability to have a setting for a custom directory 
							  'Select a new file, or analyze again with new settings.')
		self.ids.load_text.text = self.loadString

	def selectLowColor(self):
		self.lowColor = self.selectColor()
		# TODO - BUTTON COLOR CHANGE
		self.ids.low_color.background_color.hsv = (self.lowColor[2] / 255, self.lowColor[1] / 255, self.lowColor[0] / 255, 1)
		
	def selectHighColor(self):
		self.highColor = self.selectColor()
		# TODO - BUTTON COLOR CHANGE
		self.ids.high_color.background_color.hsv = (self.highColor[2] / 255, self.highColor[1] / 255, self.highColor[0] / 255, 1)
		
	def test(self):
		self.analyzer = bioAnalyzer(self.filename[0])
		l_red = np.array([0, 97, 30]) # THESE STILL NEED MASSAGING
		u_red = np.array([10, 255, 255]) # THESE STILL NEED MASSAGING
		size = [5, 20]
		color = [l_red, u_red]
		organism = "herpaderp"
		self.analyzer.analyzeOrganism(organism, size, color)


	def selectColor(self):
		img = cv2.imread(self.filename[0])
		hsv = cv2.cvtColor(img, cv2.COLOR_BGR2HLS_FULL)
		self.colors = []
		self.loc = []
		cv2.imshow('frame', img)
		while True:
			if self.colors:
				cv2.putText(img, str(self.colors[-1]), (10, 50), cv2.FONT_HERSHEY_PLAIN, 2, (255, 255, 255), 2)
			cv2.setMouseCallback('frame', self.on_mouse_click, hsv)
			if cv2.waitKey(0):
				break
		cv2.destroyAllWindows()
		if self.colors:
			return self.colors[-1]
		else: 
			return None


	def on_mouse_click (self, event, x, y, flags, frame):
		if event == cv2.EVENT_LBUTTONUP:
			self.colors.append(frame[y,x].tolist())

	def on_mouse_hover (self, event, x, y, flags, frame):
		# Create a rectangle which displays 
		# a local zoomed portion of the 
		# original image, near the mouse position
		pass


			

class BioFrame(App):
	#pass
	Root().startup()

Factory.register('Root', cls=Root)
Factory.register('LoadDialog', cls=LoadDialog)

