import pandas as pd
import numpy as np
import plotly
import os
import time
import shutil
import plotly
import plotly.plotly as py
import soundfile as sf
import speech_recognition as sr
import matplotlib.pyplot as plt

#import sa as sa
from sklearn.model_selection import train_test_split,GridSearchCV
from sklearn import model_selection
from sklearn.metrics import classification_report,accuracy_score,confusion_matrix
from sklearn.linear_model import LogisticRegression
from sklearn import tree
#from sklearn.tree import DecisionTreeClsassifier
from sklearn.neighbors import KNeighborsClassifier
from sklearn.discriminant_analysis import LinearDiscriminantAnalysis
from sklearn.naive_bayes import GaussianNB
from sklearn.svm import SVC
from pyAudioAnalysis import audioBasicIO
from pyAudioAnalysis import audioFeatureExtraction
from pyAudioAnalysis.audioFeatureExtraction import stFeatureExtraction
from pydub import AudioSegment


F = open("C:\\Users\\Rana\\Documents\\Visual Studio 2015\\Projects\\new vid\\filepath.txt",'r') 
path=F.read()
path1=path[51:-4]
#print(path)
#print(path1)
fullpathavg="C:\\Users\Rana\Desktop\\avgdata\\audio-classification\\" + path1 + ".wav"
#fullpath="C:\\Users\Rana\Desktop\\data\\audio-classification\\"+path1+".wav"


sound = AudioSegment.from_mp3(path)
sound.export(fullpathavg, format="wav")


D= pd.read_csv('C:\\Users\\Rana\\Downloads\\FinalData.csv')
D.drop(['Unnamed: 0'],axis=1,inplace=True)
X = D.drop(['Emotion'],axis=1)
Y = D['Emotion']
X_train,X_test,Y_train,Y_test = train_test_split(X,Y,test_size=0.3,random_state=101)
LDA = LinearDiscriminantAnalysis()
LDA.fit(X_train,Y_train)
predictions = LDA.predict(X_test)
#print(classification_report(Y_test,predictions))
F = []



def predictEmotion(string):
    s = sf.SoundFile(string)
    duration = len(s) / s.samplerate
    [Fs,x] = audioBasicIO.readAudioFile(string)
    x = audioBasicIO.stereo2mono(x)
    F = stFeatureExtraction(x,Fs,Fs*0.050,Fs*0.025)
    F = np.mean(F,axis=1)
    F = np.reshape(F,(1,34))
   
    predict1 = LDA.predict(F)
    #print("predict1: ")
    #print(predict1)
    predict = LDA.predict_proba(F)
    return predict


x=predictEmotion(fullpathavg)
y=['A','E','F','L','N','T','W']
y1=["Fear","Disgust","Happy","Bored","Neutral","Sad","Anger"]
ind = np.argmax(x)
#print(x)
print(y1[ind])
print(x[0][ind]*100)