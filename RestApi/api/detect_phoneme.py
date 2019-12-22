import speech_recognition as sr
import os

import subprocess
 
#from phonemizer.phonemize import phonemize as ph
#from scipy.io import wavfile
PHONES = ["ae1", "aa1", "ao1", "eh1", "er1", "iy1", "ih1", "ao1", "uw1", "uh1", "ah1"]

def call(cmd):

		## call date command ##
	p = subprocess.Popen(cmd, stdout=subprocess.PIPE, shell=True)
	
	## Talk with date command i.e. read data from stdout and stderr. Store this info in tuple ##
	## Interact with process: Send data to stdin. Read data from stdout and stderr, until end-of-file is reached.  ##
	## Wait for process to terminate. The optional input argument should be a string to be sent to the child process, ##
	## or None, if no data should be sent to the child.
	(output, err) = p.communicate()
	
	## Wait for date to terminate. Get return returncode ##
	p_status = p.wait()
	#print("Command output : ", output.decode("utf-8", "replace"))
	#print("Command exit status/return code : ", p_status)
	return str(output.decode("utf-8", "replace"))
	#os.system(cmd)

# obtain audio from the microphone

def get_phonemes(aud):

	r = sr.Recognizer()
	demo=sr.AudioFile(aud)
	print("File detected!")
	with demo as source: 
		audio=r.listen(source)
	try:
		word = r.recognize_google(audio)
	except:
		word = ""
	print("Audio: ",audio)
	#res = ph(word)

	print("Word: ",word)
	res = call('t2p "' + word + '"')

	#myfile = open("tmp.txt", 'r')
	# Write a line to the file
	#res = myfile.read()
	#myfile.close()
	
	print("Phones: ",res)
	# Close the file
	return res, word

	
def near_phones(idx):

	if(idx == 0):
		return ["ah1", "aa1"]
	elif(idx == 1):
		return ["ao1", "ae1"]
	elif(idx == 2):
		return ["ah1", "aa1"]
	elif(idx == 3):
		return ["er1", "ae1"]
	elif(idx == 4):
		return ["eh1",  "ae1"]
	elif(idx == 5):
		return ["ih1"]
	elif(idx == 6):
		return ["iy1"]
	elif(idx == 7):
		return ["uw1", "uh1"]
	elif(idx == 8):
		return ["uh1", "ao1"]
	elif(idx == 9):
		return ["uw1", "ao1"]
	elif(idx == 10):
		return ["ae1", "ao1", "aa1"]

def get_score(aud_name, target_idx):
	score = 0
	pronounced_phones, word = get_phonemes(aud_name)
	#print(pronounced_phones.split(' '))
	if PHONES[target_idx] in pronounced_phones and len(word.split(' ')) == 1:
		score += 3
	elif PHONES[target_idx] in pronounced_phones and len(word.split(' ')) > 1:
		score += 1
	elif PHONES[target_idx] not in pronounced_phones and PHONES[target_idx] in near_phones(target_idx):
		score += 2
	return(score,[str(PHONES.index(x)).zfill(2) for x in pronounced_phones.split(' ') if x in PHONES])