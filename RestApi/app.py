from flask import Flask, jsonify, request
from flask_sqlalchemy import SQLAlchemy
import io
import sys
import os
import boto3
from scipy.io import wavfile as wav
import numpy as np
from werkzeug.utils import secure_filename
#from detect_phoneme import get_score
from audio_to_phonemes import get_score
#from flask_login import LoginManager

app = Flask(__name__, instance_relative_config=False)

db = SQLAlchemy(app)
port = int(os.environ.get("PORT", 5000))

app.config['SQLALCHEMY_DATABASE_URI'] = "XXX"
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = True
app.config['UPLOAD_FOLDER'] = ""

aws_key_id='XXX'
aws_key_id_pwd='XXX'
bucket = 'audiobucket-explora-app'


@app.route('/')
def hello_world():
   return 'Explora Tablet API' #  \n Autheticated: ' + str(authenticated)

@app.route('/set_credentials')
def autenthicate():
    global aws_key_id, aws_key_id_pwd
    args = request.args
    #app.config['SQLALCHEMY_DATABASE_URI'] = args["databse_uri"]
    aws_key_id = args["aws_key_id"]
    aws_key_id_pwd = args["aws_key_id_pwd"]
    if args["aws_key_id"] and  args["aws_key_id_pwd"]:
        #authenticated = True
        return "Successfully acredited"
    else:
        return "Invalid credentials"


#Login function
@app.route('/list')
def fetch_data():
    args = request.args
    if(args["key"] == "mykey"):
        fetch_result = userlist.query.all()
        print(fetch_result)
        if "user_name" in args:
            u_name = args["user_name"]
            fetch_result = userlist.query.filter(userlist.user_name == u_name).all()
            if(fetch_result): 
                return(fetch_result[0].toJSON())
            else:
                return("USER NOT FOUND")
        else:
            fetch_result = userlist.query.all()
        #print(fetch_result)
        print(jsonify([str(result) for result in fetch_result]))
        return jsonify(users = [str(result) for result in fetch_result])
    else:
        return "You don't have valid credentials"
#Leaderboard function
@app.route('/leaderboard')
def leaderboard():
    args = request.args
    #u_name = args["user_name"]
    fetch_result_e = fetch_result = userlist.query.filter(userlist.avatar % 3 == 0).all()
    fetch_result_a = fetch_result = userlist.query.filter(userlist.avatar % 3 == 1).all()
    fetch_result_w = fetch_result = userlist.query.filter(userlist.avatar % 3 == 2).all()
    result0 = sum([x.score for x in fetch_result_e])
    result1 = sum([x.score for x in fetch_result_a])
    result2 = sum([x.score for x in fetch_result_w])
    return(jsonify(escore = result0, ascore = result1, wscore = result2));

    """if(fetch_result_e and fetch_result_a and fetch_result_e): 
        resilt
    	return(fetch_result[0].toJSON())
    else:
    	return("ERROR")"""

@app.route('/uploadfile',methods=['GET','POST'])
def uploadfile():

    print("uploading file...",request.files['file'])
    args = request.args
    print(args["user_name"])
    print(args["current_stimuli"])
    if request.method == 'POST':
        f = request.files['file']
        if f:
            filePath = "audios/" +  args["current_stimuli"] + args["user_name"] + secure_filename(f.filename)
            f.save(filePath)
            resp = upload_file_AWS(filePath, bucket)
            print(resp)

        #binary = f.read()
       
            score, phonemes = get_score(args["current_stimuli"] + args["user_name"] + secure_filename(f.filename), int(args["current_stimuli"][1] + args["current_stimuli"][2]))
            print(score, phonemes)
            return("P" + str(len(phonemes)) + str(score) + ''.join(phonemes))

def upload_file_AWS(file_name, bucket):
    """
    Function to upload a file to an S3 bucket
    """
    object_name = file_name
    s3_client = boto3.client(
        's3',
        # Hard coded strings as credentials, not recommended.
        aws_access_key_id=aws_key_id,
        aws_secret_access_key=aws_key_id_pwd
    )
    response = s3_client.upload_file(file_name, bucket, object_name)

    return response

def download_file_AWS(file_name, bucket):
    """
    Function to download a given file from an S3 bucket
    """
    s3 = boto3.resource('s3')
    output = "downloads/{file_name}"
    s3.Bucket(bucket).download_file(file_name, output)

    return output

#Register function
@app.route('/list_add', methods=['POST'])
def insert_user():
  
    data = request.get_json()
    #print(data)
    sys.stdout.flush()
    new_user_name = data['user_name']
    new_score = data['score']
    new_mode = data['mode']
    new_avatar = data['avatar']
    new_bag = data['bag']

    incompat_users = userlist.query.filter(userlist.user_name == new_user_name).all()
    #print(incompat_users)
    if(incompat_users):
    	return "Username Alredy in use"
    else:
        user = userlist(new_user_name, 1, new_score, new_mode, new_avatar, new_bag)
        db.session.add(user)
        db.session.commit()
    return "OK"

#Save progress function
@app.route('/list_save', methods=['POST'])
def save_user_progress():
    
    data = request.get_json()
    print(data)
    sys.stdout.flush()
    new_user_name = data['user_name']
    new_score = data['score']
    new_lvl = data['lvl']
    new_bag = data['bag']
    new_history = data['stimuli_history']
    new_answers = data['answers']
    users = userlist.query.filter(userlist.user_name == new_user_name).all()
    #print(incompat_users)
    if(not users):
   	    return "Error while saving data"
    else:
        user = userlist.query.filter(userlist.user_name == new_user_name).update(dict(score=new_score,lvl = new_lvl,bag = new_bag, stimuli_history = new_history, answers = new_answers))
	    #user = userlist(new_user_name, new_score)
	    #db.session.add(user)
        db.session.commit()
    return "OK"
      
class userlist(db.Model):
    user_id = db.Column(db.Integer, primary_key=True, nullable=False, autoincrement=True)
    user_name = db.Column(db.String(80), nullable=False)
    lvl = db.Column(db.Integer, unique=False, nullable=True)
    score = db.Column(db.Integer, unique=False, nullable=True)
    mode = db.Column(db.Integer, unique=False, nullable=True)
    avatar = db.Column(db.Integer, nullable=False)
    bag = db.Column(db.String(8*8), nullable=False)
    stimuli_history = db.Column(db.String(1500), nullable=False)
    answers = db.Column(db.String(4300), nullable=False)
    
    #utype = db.Column(db.String(80), nullable=True)
    #mode = db.Column(db.Integer, unique=False, nullable=True)

    def __init__(self, new_user_name, new_lvl, new_score, new_mode, new_avatar, new_bag):
        self.user_name = new_user_name
        self.lvl = new_lvl
        self.score = new_score
        self.mode = new_mode
        self.avatar = new_avatar
        self.bag = new_bag
        self.stimuli_history = ""
        self.answers = ""
     
    def __repr__(self):
    	#print(str())
    	#return str(jsonify(userId = self.user_id,userName=self.user_name, score = self.score))
        return '{' + '"user_id": {0}, "user_name": "{1}", "lvl": {2}, "score" : {3}, "mode" : {4}, "avatar" : {5}, "bag" : "{6}",  "stimuli_history" : "{7}",  "answers" : "{8}"'.format(self.user_id, self.user_name, self.lvl, self.score, self.mode, self.avatar, self.bag, self.stimuli_history, self.answers) + '}'
        
    def toJSON(self):
    	return jsonify(user_id = self.user_id,
                        user_name=self.user_name,
                        score = self.score,
                        lvl = self.lvl,
                        mode = self.mode,
                        avatar = self.avatar,
                        bag = self.bag,
                        stimuli_history = self.stimuli_history,
                        answers = self.answers)

if __name__ == "__main__":
    app.run(threaded=True, debug=True, host='0.0.0.0', port=port)
