# ucount 
ucount is a CLI-tool built to calculate the approximate coverage of organisms in an image. 

ucount is currently only configured for CFM (Confocal Microscope) images, and are configured
via a config.json file. 

# Installation
Below are instructions on how to install ucount on your machine. 

## Linux and Mac
### Steps to install

1.) Open a terminal

2.) In the terminal, move to your desired destination directory and enter the following commands in order (without the '$ '):
- git clone git@github.com:alistair-mclean/ucount.git
- cd ucount && pip install -r requirements.txt && pip install -e .

## Windows
### Before you install
Before you install, Python and its package manager (pip) must be installed. If both of these are already on your machine, you may continue onto the installation steps.

1.) Go here to download and install Python (3.7+) for your machine: https://www.python.org/downloads/

2.) Open a command prompt and enter:
- curl https://bootstrap.pypa.io/get-pip.py -o get-pip.py
- python get-pip.py

### Steps to install
1.) From this page: 
- Click on "Clone or download", and download the Zip.
- Extract the contents of the zip 

2.) In the command prompt, move to the location of the extracted contents and enter the following commands in order (without the '$ '):
- pip install -r requirements.txt && pip install -e .


# Usage
For single file execution, run the following command:
- ucount --config path/to/config --filename path/to/file

For executing ucount on a whole directory (or folder), run the following command:
- ucount --directory path/to/folder

A config is necessary but the config argument is optional, and will default to the config in the directory you’re analyzing if there is one. However if none exist, and one is not provided through arguments; ucount will fail. 

## config.json
The config.json file is used to configure ucount for custom execution. A config file should contain the following structure:
```json
{
    "organisms" : [
		{
			"name" : "Pseudomonas Aeruginosa",
			"config": {
				"filter mode": "HSV",
				"preprocessing" : {
					"filter values": {
						"min": [40, 0, 0],
						"max": [180, 255, 255]
					},
					"clahe" : {
						"on": true,
						"clip_limit": 1.5,
						"k_size": 10
					},
					"blur" : {
						"on": true,
						"k_size": 3
					}
				},
				"analysis" : {
					"threshold" : {
						"min": 45,
						"max": 255
					}
				}

			}
		},
		{
			"name" : "E Coli",
			"config": {
				"filter mode" : "BGR",
				"preprocessing" : {
					"filter values": {
						"min": [0, 0, 10],
						"max": [0, 0, 255]
					},
					"clahe" : {
						"on": true,
						"clip_limit": 1.5,
						"k_size": 10
					},
					"blur" : {
						"on": true,
						"k_size": 3
					}
				},
				"analysis" : {
					"threshold" : {
						"min": 35,
						"max": 255
					}
				}

			}
		}
    ],
	"image settings": {
		"width": 212,
		"height": 212,
		"units": "um",
		"zoom factor": 1
	}
}
```

- organisms: A list of organism configurations for analysis. Configuration of the analysis and preprocessing is defined by the user for each organism. 
- image settings: The image settings of the capture from the microscope. These values assist with the calculation for the approximate coverage.  

### organism config
#### filter mode
There are a 3 possibilities for segmenting images:
##### GRY
Grayscale. Use this if your image only contains 1 type of organism.
- Value ranges:  The min and max for value range is then between 0 and 255
##### BGR
(**NOT RECOMMENDED TO USE THIS**) Blue, Green, Red; typically used if only one of the channels is represented as an organism.
- Value ranges: The min and max value range is [0, 0, 0] [255, 255, 255]. (For segmenting Red one would enter min: [0, 0, 0], max: [0, 0, 255])
##### HSV
Hue, Saturation, Value. Used for images wiht color distributions representing organisms. Especially if there are multiple organisms on one image. 
- Value ranges: The min and max value range is [0, 0, 0] [180, 255, 255]. (For segmenting Blue one would enter min: [40, 0, 0], max: [180, 255, 255])

#### preprocessing
Preprocessing is optional but can provide better results for certain situations. There are two preprocessing stages:
- Gaussian blur: Used for improving signal to noise ratio. I've found that a kernel size of [3,3] typically works the best. 
- CLAHE: Used for improving the contrast for images that are either especially dim, or especially bright. 
