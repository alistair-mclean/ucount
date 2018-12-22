all: uCounter

uCounter: main.o imageprocessor.o preprocessor.o statistics.o
	g++ main.o imageprocessor.o preprocessor.o statistics.o -o uCounter

main.o:
	g++ -c Main.cpp

imageprocessor.o:
	g++ -c ImageProcessor.cpp

preprocessor.o:
	g++ -c PreProcessor.cpp

statistics.o:
	g++ -c Statistics.cpp


