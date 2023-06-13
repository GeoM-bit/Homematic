/*
  MySQL Connector/Arduino Example : basic select

  This example demonstrates how to issue a SELECT query with no parameters
  and use the data returned. For this, we use the Cursor class to execute
  the query and get the results.

  It demonstrates who methods for running queries. The first allows you to
  allocate memory for the cursor and later reclaim it, the second shows how
  to use a single instance of the cursor use throughout a sketch.

  NOTICE: You must download and install the World sample database to run
          this sketch unaltered. See http://dev.mysql.com/doc/index-other.html.

  CAUTION: Don't mix and match the examples. Use one or the other in your
           own sketch -- you'll get compilation errors at the least.

  For more information and documentation, visit the wiki:
  https://github.com/ChuckBell/MySQL_Connector_Arduino/wiki.

  INSTRUCTIONS FOR USE

  1) Change the address of the server to the IP address of the MySQL server
  2) Change the user and password to a valid MySQL user and password
  3) Connect a USB cable to your Arduino
  4) Select the correct board and port
  5) Compile and upload the sketch to your Arduino
  6) Once uploaded, open Serial Monitor (use 115200 speed) and observe

  Note: The MAC address can be anything so long as it is unique on your network.

  Created by: Dr. Charles A. Bell
*/
#include <Ethernet.h>
#include <MySQL_Connection.h>
#include <MySQL_Cursor.h>

byte mac_addr[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };

IPAddress server_addr(34,30,254,246);  // IP of the MySQL *server* here
char user[] = "root";              // MySQL user login username
char password[] = "1234";        // MySQL user login password

// Sample query
char query[] = "SELECT temperature,light_intensity,opened_door, temperature_ESP FROM HomeAutomation.Parameters, HomeAutomation.Temperature_ESP;";

EthernetClient client;
MySQL_Connection conn((Client *)&client);
// Create an instance of the cursor passing in the connection
MySQL_Cursor cur = MySQL_Cursor(&conn);

//#include <DHT.h>
//#define DHTTYPE DHT11
//#define DHTPIN A0
//#include <DHT_U.h>
//DHT_Unified dht(DHTPIN, DHTTYPE);
//sensors_event_t event;

//dht senzor temp
#include <DHT.h>
#define DHTPIN A0
#define DHTTYPE DHT11
DHT dht(DHTPIN, DHTTYPE);

//variabile declarate
const int pinLight = 3;
const int pinDoor = 4;
const int pinTemp = 5;
//int pinTempSensor = A0;
float currentTemp;


void setup() {
  Serial.begin(115200);
  while (!Serial); // wait for serial port to connect
  Ethernet.begin(mac_addr);
  Serial.println("Connecting...");
  if (conn.connect(server_addr, 3306, user, password)) {
    delay(1000);
  }
  else
    Serial.println("Connection failed.");
  pinMode(pinLight,   OUTPUT);
  pinMode(pinDoor, OUTPUT);
  pinMode(pinTemp,  OUTPUT);
//  pinMode(pinTempSensor, INPUT);
  dht.begin();
}


void loop() {
  currentTemp = dht.readTemperature();  // Read temperature in Celsius
    if (isnan(currentTemp)) {
    Serial.println("Error reading temperature!");
  }
  else{
    Serial.print("Temperatura curenta: ");
    Serial.print(currentTemp);
    Serial.println(" °C");
  }
  row_values *row = NULL;
  int lumina;
  int usa;
  float tempSelectata,tempESP;

  delay(1000);

  Serial.println("1) Demonstrating using a cursor dynamically allocated.");
  // Initiate the query class instance
  MySQL_Cursor *cur_mem = new MySQL_Cursor(&conn);
  // Execute the query
  cur_mem->execute(query);
  // Fetch the columns (required) but we don't use them.
  column_names *columns = cur_mem->get_columns();

  // Read the row (we are only expecting the one)
  do {
    row = cur_mem->get_next_row();
    if (row != NULL) {
      tempSelectata = atof (row->values[0]);
      lumina = atol(row->values[1]);
      usa = atol(row->values[2]);
      tempESP = atof (row->values[3]);
      
    }
  } while (row != NULL);
  // Deleting the cursor also frees up memory used
  delete cur_mem;

  // Show the result
  Serial.print("  Lumina = ");
  Serial.println(lumina);
  Serial.print("  Temperatura selectata = ");
  Serial.println(tempSelectata);
  Serial.print("  Stare usa = ");
  Serial.println(usa);
  Serial.print("  Temperatura ESP = ");
  Serial.println(tempESP);

  // setare starea usii
     if(usa)
        digitalWrite(pinDoor, HIGH); 
     else
        digitalWrite(pinDoor, LOW);

  // setare lumina  
      analogWrite(pinLight, lumina*100 / 255);

  //temperatura
  if(((tempSelectata - currentTemp) > 1) || ((tempSelectata - tempESP) > 1)){
          digitalWrite(pinTemp,HIGH);
  }
  else{
      digitalWrite(pinTemp,LOW);
  }
  
  delay(10000);

}
