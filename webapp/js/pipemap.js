class PipeMap {
	constructor(owner, transportMaterial, map) {
		if(typeof(owner) != 'string'){
			//TODO - ADD AN ERROR MESSAGE
			break;
		}
		if (typeof(transportMaterial) != 'string') {
			//TODO - ADD AN ERROR MESSAGE
			break;
		}
		if(typeof(map) != 'object') {
			//TODO - ADD AN ERROR MESSAGE
			break; 
		}
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
    function initMap() {
      map = new google.maps.Map(document.getElementById('map'), {
    	// TODO - make function to get user location (if able)
      center: {lat: 40.025040, lng: -105.285787},
      zoom: 13
    });
};

var Pipe = function(location, sections[],depth) {
	if(typeof(location) != 'object') {
		//TODO - ERROR MESSAGE HERE
		break;
	}
	for (let pipeSection in sections){

	}
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