CONFIG = {
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
							"on": True,
							"clip_limit": 1.5,
							"k_size": 10
						},
						"blur" : {
							"on": True,
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
							"on": True,
							"clip_limit": 1.5,
							"k_size": 10
						},
						"blur" : {
							"on": True,
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