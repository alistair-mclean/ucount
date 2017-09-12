var factorize = function(n) {
	var results = [];
	var i = 2;
	while (n > 1) {
		if (n % i ===0) {
		n /= 1;
		results.push(i);
	} else {
		i++;
	}
	}
	return results;
	};

class PipeLine {
	constructor(owner, transportMaterial, pipeMap) {
		this.owner = owner;
		this.transportMaterial = transportMaterial;
		this.pipeMap = pipeMap;
	}

	get owner() {
		return owner;
	}

	get transportMaterial() {
		return transportMaterial;
	}

	loadMap() {
		
	}



};

var Pipe = function(location,depth) {

};

     var map;
      function initMap() {
        map = new google.maps.Map(document.getElementById('map'), {
          center: {lat: 40.025040, lng: -105.285787},
          zoom: 13
        });
      var markerOptions = {
      	position:new google.maps.LatLng(40.025040, -105.285787)
      };
      var marker = new google.maps.Marker(markerOptions);
      marker.setMap(map);

      //Temporary quick way to test pipe locations on map
      var jointLocations = [{lat: 40.024268, lng:-105.285787},
                            {lat: 40.023808, lng:-105.285787},
                            {lat: 40.023216, lng:-105.285766},
                            {lat: 40.021951, lng:-105.285079},
                            {lat: 40.020686, lng:-105.285015},
                            {lat: 40.020944, lng:-105.283757},
                            {lat: 40.019169, lng:-105.284959}]
      
        var pipeLine = new google.maps.Polyline({
          path: jointLocations,
          geodesic: true,
          strokeColor: '#4286f4',
          strokeOpacity: 1.0,
          strokeWeight: 2
        });

        pipeLine.setMap(map);
      };