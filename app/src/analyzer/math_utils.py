import numpy as np
from pprint import pprint 

def compute_coverage(image, image_settings):
    """
    
    Args:
        image (TYPE): ndarray
    Returns:
        TYPE: Dictionary
    """
    height, width = np.shape(image)
    area = height * width
    zoom_factor = image_settings['zoom factor']
    approx_pixel_area = area / ( image_settings['height'] * image_settings['width'] ) * zoom_factor
    approx_area_unit = image_settings['units'] + '^2'
    sum_of_pixels = 0 
    for row in image:
        for px in row:
            if px > 245:
                sum_of_pixels += 1
    percent_covered = float(sum_of_pixels) / float(area) * 100.0
    approximate_coverage = percent_covered * approx_pixel_area
    results = {
        'Percent coverage' : float('%.2f' % percent_covered),
        'Image resolution' : [height, width],
        'Approximate coverage' : float('%.3f' % approximate_coverage),
        'Coverage units': approx_area_unit,
    }
    return results

def calculate_coverage_ratios(organism_results):
    pprint(organism_results)
    input('Waiting so you can debug')