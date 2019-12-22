from os import environ, path
from pocketsphinx.pocketsphinx import *
from sphinxbase.sphinxbase import *
import sys

MODELDIR = "pocketsphinx/model"
DATADIR = "audios"
VOWELS = ["AE", "AY", "AW", "AA", "AO", "EH", "EH_AE", "EY", "OW", "ER", "IH", "IY", "IY_EA", "AO_R", "OY", "UW", "UH", "UH_AE", "AH"]

def get_score(filename, stimuli_idx):
# Create a decoder with certain model
  config = Decoder.default_config()
  config.set_string('-hmm', path.join(MODELDIR, 'en-us/my-en-us'))
  config.set_string('-allphone', path.join(MODELDIR, 'en-us/en-us-phone.lm.bin'))
  config.set_float('-lw', 2.0)
  config.set_float('-beam', 1e-10)
  config.set_float('-pbeam', 1e-10)

  # Decode streaming data.
  decoder = Decoder(config)

  decoder.start_utt()
  stream = open(path.join(DATADIR, filename), 'rb')
  while True:
    buf = stream.read(1024)
    if buf:
      decoder.process_raw(buf, False, False)
    else:
      break
  decoder.end_utt()

  hypothesis = decoder.hyp()
  pronounced_phonemes =  [seg.word for seg in decoder.seg() if seg.word in VOWELS]
  print('Pronounced Phonemes: ', [seg.word for seg in decoder.seg() if seg.word in VOWELS])

  correct_vowel = VOWELS[stimuli_idx]
  score = 0
  if(len(correct_vowel) == 2):
    if(correct_vowel in pronounced_phonemes):
      score = 1
    else:
      score = 0
  else:
    cv1 = correct_vowel[:2]
    cv2 = correct_vowel[3:] 
    for i in range(len(pronounced_phonemes) - 1):
      if(pronounced_phonemes[i] == cv1 and pronounced_phonemes[i + 1] == cv2):
        score = 1
        break
      else:
        score = 0
        break

  return(score, [str(VOWELS.index(p)).zfill(2) for p in pronounced_phonemes])

#get_score("audiofile.wav",0)