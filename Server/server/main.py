import socket
import pyodbc
import datetime
import AlgOne
import AlgTwo
import AlgThree
import AlgFour
import AlgFive
import subprocess

print("Подключение к базе данных")
try:
    dbcon = pyodbc.connect("Driver={SQL Server Native Client 11.0};"
                      "Server=localhost;"
                      "Database=CourseDatabase;"
                      "Trusted_Connection=yes;")
    print("Подключение к базе данных успешно")

except:
    print("Подключение к базе данных не удалось")

# Задаем адрес сервера
SERVER_ADDRESS = ('localhost', 8686)

# Настраиваем сокет
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.bind(SERVER_ADDRESS)
server_socket.listen(10)
print('Сервер запущен, ожидаются подклюючения')

# Слушаем запросы
while True:
    connection, address = server_socket.accept()
    print("Новое подключение клиента {address}".format(address=address))
    connection.send(bytes("Запрос на прогноз принят в обработку", encoding='UTF-16'))
    data = connection.recv(1024)
    print("Новый запрос от клиента: " + str(data, encoding='UTF-16'))
    print("Обработка запроса: " + " | " + str(data, encoding='UTF-16') + " | ")
    parameters = str(data, encoding='UTF-16')
    values = parameters.split(',')
    date = values[0]
    time = values[1]
    UserId = values[2]
    DeviceId = values[3]
    dateTime = datetime.datetime.strptime(date+ " " + time, '%Y-%m-%d %H:%M:%S')
    AlgOne.main(dateTime, DeviceId, UserId)
    AlgTwo.main(dateTime, DeviceId, UserId)
    AlgThree.main(dateTime, DeviceId, UserId)
    AlgFour.main(dateTime, DeviceId, UserId)
    AlgFive.main(dateTime, DeviceId, UserId)
    print("Прогнозы выполнены, обновите страницу клиента")
    print("Ожидание новых подключений…")

