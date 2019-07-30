from setuptools import setup
setup(
    name = 'ucount',
    version = '0.1.0',
    packages = ['ucount'],
    entry_points = {
        'console_scripts': [
            'ucount = ucount.__main__:parse_arguments'
        ]
    })