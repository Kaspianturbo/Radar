//Підключення бібліотеки Servo.h для роботи з сервоприводо
#include <Servo.h> 

//Оголошення цілих змінних які місстять номер піна.
const int trigPin = 2;
const int echoPin = 3;

//Змінні для зберігання куту повороту і відстані відповідно
long duration;
int distance;
int angle;

//Оголошуємо змінну типу Servo
Servo myServo;
//Функція яка виконується при увімкненні мікроконтролера
void setup()
{
  //Вказуємо, що на пін trig буде подаватися напруга 
  //а з піна echo буде проводитися зчитування
  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
  //Вказуємо швидкість передачі даних по COM порту
  Serial.begin(9600);
  //Вказуємо, що керування сервоприводом буде 
  //здійснюватися по піну номер 4
  myServo.attach(4);
  //Встановлюємо початковий кут
  angle = 10;
  //Функція write об'єкту типу Servo задає кут 
  //на який буде розвернутий сервопривод
  myServo.write(angle);
  //Встановлюємо затримку
  delay(100);
}
//Функція яка циклічно виконується поки плата увімкнена
void loop()
{
  for(angle; angle < 180; angle+=1)
  {
    myServo.write(angle);
    distance = calculateDistance();
    delay(30);

    Serial.print(angle);
    Serial.print(",");
    Serial.print(distance);
    Serial.print(".");
  }
  for(angle; angle > 0; angle-=1)
  {
    myServo.write(angle);
    distance = calculateDistance();
    delay(30);
    
    Serial.print(angle);
    Serial.print(",");
    Serial.print(distance);
    Serial.print(".");
  }
}
//Функція для обрахування відстані який вертає датчик
int calculateDistance()
{
  //На trig подається 5В протягом 10 мілісекунд для 
  //створення короткого ультразвукового імпульсу.
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);  
  digitalWrite(trigPin, LOW);
  //Функція pulseIn вертає відстань в дюймах яку вертає нам датчик
  //Інформація зчитується з echo.
  duration = pulseIn(echoPin, HIGH);
  //Переведення в сантиметри
  distance = duration*0.034/2;
  return distance;
}
