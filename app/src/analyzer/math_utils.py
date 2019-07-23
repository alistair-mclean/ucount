import numpy as np
from pprint import pprint 

def compute_coverage(image):
    """
    
    Args:
        image (TYPE): ndarray
    Returns:
        TYPE: Dictionary
    """
    height, width = np.shape(image)
    area = height * width
    sum_of_pixels = 0 
    for row in image:
        for px in row:
            if px > 245:
                sum_of_pixels += 1
    percent_covered = float(sum_of_pixels) / float(area) * 100.0
    results = {
        'Percent coverage' : float('%.2f' % percent_covered),
        'Image resolution' : [height, width],
    }
    return results

def calculate_coverage_ratios(organism_results):
    pprint(organism_results)
    input('Waiting so you can debug')