from array import *
import pyodbc
import datetime
from dataclasses import dataclass, field
class CoefClass:
    Avalue: float = 0
    Bvalue: float = 0
    ResultRmse: float = 0
    ResultAlgoritm: float = 0

hoursInDay = 24
Dates = []
Values =[]
ResultValues =[]
CountOfElements = 0
RmseDates = []
RmseValues =[]
ResultsAglOne = []
ResultsAlgTwo = []

def byRmse_key(CoefClass):
    return CoefClass.ResultRmse

def SearchRmseForOne(RealValue, ResValue):
    result = ((RealValue - ResValue) ** 2) ** 0.5
    return result

def SeachNumberLong(inputDate):
    result = 0
    counter = 0
    for i in Dates:
        counter+= 1
        if i.strftime('%Y-%m-%d %H') == inputDate.strftime('%Y-%m-%d %H'):
            result = counter
    return result
def SeachNumberShort(inputDate, dates):
    result = 0
    counter = 0
    for i in dates:
        counter += 1
        if i.strftime('%Y-%m-%d') == inputDate.strftime('%Y-%m-%d'):
            result = counter
    return result
def CalculateRMSE(values, count):
    result = 0
    temp = 0
    for i in range(0, count-1):
        temp += ((Values[i]-values[i]) ** 2) ** 0.5
        if i % 24 == 0:
            temp /= 24
            RmseValues.append(temp)
            temp = 0

def forecast(values,count, values1, values2):
    result = 0
    a = 0.5
    b = 0.5
    for i in range(0, count-1):
        ResultValues.append(a*values1[i]+b*values2[i])
    masRes =[]
    for i in range(0, 6):
        masRes.append(CoefClass())
    FixRmse = 100
    TempRmse = 99
    for i in range(0, count-1):
        while True:
            if TempRmse < FixRmse:
                FixRmse = TempRmse
            # ============================================================================
            a += 0.01
            masRes[0].Avalue = a
            masRes[0].Bvalue = b
            masRes[0].ResultAlgoritm = a * values1[i] + b * values2[i]
            masRes[0].ResultRmse = SearchRmseForOne(values[i], masRes[0].ResultAlgoritm)
            # ============================================================================
            b += -0.01
            masRes[1].Avalue = a
            masRes[1].Bvalue = b
            masRes[1].ResultAlgoritm = a * values1[i] + b * values2[i]
            masRes[1].ResultRmse = SearchRmseForOne(values[i], masRes[1].ResultAlgoritm)
            a += -0.01
            b += 0.01
            # ============================================================================
            b += 0.01
            masRes[2].Avalue = a
            masRes[2].Bvalue = b
            masRes[2].ResultAlgoritm = a * values1[i] + b * values2[i]
            masRes[2].ResultRmse = SearchRmseForOne(values[i], masRes[2].ResultAlgoritm)
            b += -0.01
            # ============================================================================
            b += -0.01
            masRes[3].Avalue = a
            masRes[3].Bvalue = b
            masRes[3].ResultAlgoritm = a * values1[i] + b * values2[i]
            masRes[3].ResultRmse = SearchRmseForOne(values[i], masRes[3].ResultAlgoritm)
            b += 0.01
            # ============================================================================
            b += 0.01
            a += -0.01
            masRes[4].Avalue = a
            masRes[4].Bvalue = b
            masRes[4].ResultAlgoritm = a * values1[i] + b * values2[i]
            masRes[4].ResultRmse = SearchRmseForOne(values[i], masRes[4].ResultAlgoritm)
            b += -0.01
            # ============================================================================
            masRes[5].Avalue = a
            masRes[5].Bvalue = b
            masRes[5].ResultAlgoritm = a * values1[i] + b * values2[i]
            masRes[5].ResultRmse = SearchRmseForOne(values[i], masRes[5].ResultAlgoritm)
            a += 0.01
            # ============================================================================
            masRes = sorted(masRes, key = byRmse_key)
            a = masRes[0].Avalue
            b = masRes[0].Bvalue
            TempRmse = masRes[0].ResultRmse
            if not (TempRmse < FixRmse and abs(TempRmse) > 0.01):
                break
        ResultValues.append(masRes[0].ResultAlgoritm)
    return result

def main(date, device, user):
    dbcon = pyodbc.connect("Driver={SQL Server Native Client 11.0};"
                      "Server=localhost;"
                      "Database=CourseDatabase;"
                      "Trusted_Connection=yes;")
    cursor = dbcon.cursor()
    cursor.execute("SELECT DateTime FROM Real_Values WHERE Device_Id = %s" % device)

    for row in cursor:
        Dates.append(row[0])
    cursor.execute("SELECT Value FROM Real_Values WHERE Device_Id = %s" % device)
    for row in cursor:
        Values.append(float(row[0]))
    cursor.execute("SELECT Value FROM Algoritm_Results WHERE Device_Id = %s AND Algoritm_Id = '1'" % device)
    for row in cursor:
        ResultsAglOne.append(float(row[0]))
    cursor.execute("SELECT Value FROM Algoritm_Results WHERE Device_Id = %s AND Algoritm_Id = '2'" % device)
    for row in cursor:
        ResultsAlgTwo.append(float(row[0]))

    CountOfElements = len(Values)
    forecast(Values, CountOfElements, ResultsAglOne, ResultsAlgTwo)
    cursor = dbcon.cursor()

    for i in range(0, CountOfElements-1):
        cursor.execute("INSERT Algoritm_Results VALUES(%s, '%s', %s, %s)" % ('3', Dates[i].strftime("%d.%m.%Y %H:%M:%S"), ResultValues[i], device))
    for i in range(0, CountOfElements-1):
        if i % 24 == 0:
            RmseDates.append(Dates[i])
    CalculateRMSE(ResultValues, CountOfElements)

    for i in range(0, len(RmseDates)):
        cursor.execute("INSERT Rmse_Values VALUES('%s', %s, %s, %s)" % (RmseDates[i].strftime("%d.%m.%Y %H:%M:%S"), RmseValues[i], '3', device))

    dateTime = date
    numberDateForExportLong = SeachNumberLong(dateTime)
    numberDateForExportShort = SeachNumberShort(dateTime, RmseDates)

    dbcon.commit()
    cursor.execute("SELECT Id FROM Algoritm_Results WHERE DateTime = '%s' AND Algoritm_id = '3'" % Dates[
        numberDateForExportLong].strftime("%d.%m.%Y %H:%M:%S"))
    for row in cursor:
        value_id = row[0] - 1
    cursor.execute(
        "SELECT Id FROM Rmse_Values WHERE Date = '%s' AND AlgId = '3'" % RmseDates[numberDateForExportShort].strftime(
            "%d.%m.%Y %H:%M:%S"))
    for row in cursor:
        rmse_value_id = row[0] - 2
    cursor.execute("SELECT Id FROM Real_Values WHERE DateTime = '%s'" % Dates[numberDateForExportLong].strftime("%d.%m.%Y %H:%M:%S"))
    for row in cursor:
        real_value_id = row[0]-1

    resultStr = f"INSERT Forecasts VALUES(%s, '%s', %s, %s, %s, %s)" % ('3', dateTime.strftime("%d.%m.%Y %H:%M:%S"), value_id, real_value_id, user, rmse_value_id)
    cursor.execute(resultStr)
    dbcon.commit()
    Dates.clear()
    Values.clear()
    ResultValues.clear()
    RmseDates.clear()
    RmseValues.clear()
    ResultsAglOne.clear()
    ResultsAlgTwo.clear()
    print(f"Выполнен прогноз алгоритмом 3 для данных ")
