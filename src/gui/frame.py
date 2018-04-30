# The module to control the file tab for UCOUNTER
# Author - Alistair McLean
# ----------------------------------------
# This tab handles the functionality to allow the 
# user to control which files they wish to analyze

# TODO  
# - Improve the color selection 
# - Add auto thresholding of the image 
# - 
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
from src.analyzer.analyzer import Analyzer
from tkinter.filedialog import askopenfilename
import tkinter as tk
import numpy as np
import ntpath
import os
import cv2

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
	loadString = "Press Load to select a file you wish to analyze."
	hasLowColor = False
	hasHighColor = False
	

	def startup(self):
		self.hasLowColor = False # Figure out better logic for this
		self.hasHighColor = False # Figure out better logic for this
	
	def dismiss_popup(self):
		if self._popup:
			self._popup.dismiss()

	def show_load(self):
#		content = LoadDialog(load=self.load, 
#							cancel=self.dismiss_popup)
#		self._popup = Popup(title="Load file", content=content,
#							size_hint=(0.9,0.9))
#		self._popup.open()
		root = tk.Tk()
		root.withdraw()
		self.filename = askopenfilename()
		if self.filename:
			self.load(self.filename)
			root.destroy()

	def colorPicker(self):
		# When the button is pressed activate this widget
	    clr_picker = ColorPicker()
	    # Add widget to the Analyzer pane. 
	    #self.ids.analyze_tab.add_widget(clr_picker)

	    # To monitor changes, we can bind to color property changes

	    clr_picker.bind(color=self.on_color)		
		
	def on_color(instance, value):
	        print ("RGBA = ", str(value))  #  or instance.color
	        print ("HSV = ", str(instance.hsv))
	        print ("HEX = ", str(instance.hex_color))
#	    	return instance.hsv

	def load(self, filename):
		self.filename = filename
		if filename:
#			fname = ntpath.basename(filename[0])
			fname = ntpath.basename(filename)
		else:
			return

		# TODO - add a check to make sure that it is an image extension!!!!
		self.loadString = str('{} loaded.\nPlease select the settings for the organism you wish to analyze.'.format(fname))
		self.ids.load_text.text = self.loadString
		#self.ids.load_image.source = filename[0]
		self.ids.load_image.source = filename

		# ENABLE Analyzer
		self.ids.analyze_tab.disabled = False
		#self.dismiss_popup()

	def analyze(self):
#		self.analyzer = bioAnalyzer(self.filename[0])
#		fname = ntpath.basename(self.filename[0])
		if self.hasHighColor != True and self.hasLowColor != True:
			self.loadString = str('Please select the low and high color thresholds for the image.')
			self.ids.load_text.text = self.loadString
			return
		self.analyzer = Analyzer(self.filename)
		fname = ntpath.basename(self.filename)
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
		self.loadString = str('Finished analyzing {}\n'.format(fname) +
							  'Select a new file, or analyze again with new settings.')
		self.ids.load_text.text = self.loadString

	def selectLowColor(self):
		self.lowColor = self.selectColor()
		if self.loc:
			kivyColor = self.img[self.loc[-1][0],self.loc[-1][1]]
			kivyColor = (kivyColor[2] / 255, kivyColor[1] / 255, kivyColor[0] / 255, 1) 
			self.ids.low_color.background_color = kivyColor 
			self.hasLowColor = True
		
	def selectHighColor(self):
		self.highColor = self.selectColor()
		if self.loc:
			kivyColor = self.img[self.loc[-1][0],self.loc[-1][1]]
			kivyColor = (kivyColor[2] / 255, kivyColor[1] / 255, kivyColor[0] / 255, 1)
			self.ids.high_color.background_color = kivyColor
			self.hasHighColor = True


	## DEBUG ##
	def test(self):
		self.analyzer = Analyzer(self.filename)
		l_red = np.array([0, 97, 30]) # THESE STILL NEED MASSAGING
		u_red = np.array([10, 255, 255]) # THESE STILL NEED MASSAGING
		size = [5, 20]
		color = [l_red, u_red]
		organism = "herpaderp" 
		self.analyzer.analyzeOrganism(organism, size, color)

	def selectColor(self):
#		self.img = cv2.imread(self.filename[0])
		self.img = cv2.imread(self.filename)
		hsv = cv2.cvtColor(self.img, cv2.COLOR_BGR2HSV)
		self.colors = []
		self.loc = []
		while True:
			cv2.imshow('Source', self.img)
			cv2.setMouseCallback('Source', self.on_mouse_click, self.img)
			if cv2.waitKey(0):
				break
		cv2.destroyAllWindows()
		if self.loc:
			return hsv[self.loc[-1][0], self.loc[-1][1]]
#		if self.colors:
#			return self.colors[-1]
		else: 
			return None


	def on_mouse_click (self, event, x, y, flags, frame):
		if event == cv2.EVENT_LBUTTONUP:
#			self.colors.append(frame[y,x].tolist())
			self.loc.append([y,x])
		elif event == cv2.EVENT_MOUSEMOVE:
			self.displayZoomedBoxWithIcon(x,y,frame,4)
			
		
	def displayZoomedBoxWithIcon(self, x, y, frame, zoom=12):
		height, width = frame.shape[:2]
		verticalPadding = int(0.1 * height)
		horizontalPadding = int(0.1 * width)
		windowSize = 30
		# Check if the x and y are greater than a certain percent
		# And auto position the display
		# Crop the frame
		crop = frame[(y - windowSize):(y + windowSize), 
					 (x - windowSize):(x + windowSize)]
		cHeight, cWidth = crop.shape[:2]
		# Enlarge the croppage 
		res = cv2.resize(crop,(zoom*cWidth, zoom*cHeight), interpolation = cv2.INTER_CUBIC)
		hsv = cv2.cvtColor(res, cv2.COLOR_BGR2HSV)
		res = self.drawCrosshair(res,50)

		px = hsv[int(cHeight/2), int(cWidth/2)]
		
		# Display update text on zoom box
		# - Location, Color: 
		message = str('Location:({},{}) HSV: {:2}'.format(x,y,str(px)))
		font = cv2.FONT_HERSHEY_SIMPLEX
		bottomLeftCornerOfText = (10,100)
		fontScale = 1
		fontColor = (255,255,255)
		lineType = 2
#		cv2.putText(res, message, bottomLeftCornerOfText, fontScale, fontColor, lineType)

		# TODO - figure out a way to put this on the img. 
		cv2.imshow('zoom', res)
		#return res

	def drawCrosshair(self, frame, lineLength):
		res = frame
		rHeight, rWidth = res.shape[:2]
		rHalfH = int(rHeight / 2)
		rHalfW = int(rWidth/ 2)

		#Black outline
		res = cv2.line(res, (rHalfH - lineLength, rHalfW), (rHalfH + lineLength, rHalfW), (0,0,0), 4)
		res = cv2.line(res, (rHalfH, rHalfW - lineLength), (rHalfH, rHalfW + lineLength), (0,0,0), 4)
		
		#White infil
		res = cv2.line(res, (rHalfH - lineLength, rHalfW), (rHalfH + lineLength, rHalfW), (255,255,255), 2)
		res = cv2.line(res, (rHalfH, rHalfW - lineLength), (rHalfH, rHalfW + lineLength), (255,255,255), 2)
		res[rHalfW-4:rHalfW+4, rHalfH-4:rHalfH+4] = frame[rHalfW-4:rHalfW+4, rHalfH-4:rHalfH+4]
		return res

	def on_mouse_move (self, event, x, y, flags, frame):
		# Create a rectangle which displays 
		# a local zoomed portion of the 
		# original image, near the mouse position
		pass

class BioFrame(App):
	def build(self):
		self.title = "uCounter v0.5a" # v0.5a @ 04-15-2018
	Root().startup()

Factory.register('Root', cls=Root)
 
