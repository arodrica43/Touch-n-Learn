FROM ubuntu:18.04

RUN apt-get update -y && \
    apt-get install -y python-pip python-dev && \
    apt-get install -y build-essential && \
    apt-get install -y swig python3-dev python3-numpy python3-scipy && \
    apt-get install -y sox bison libpulse-dev libasound2-dev

# We copy just the requirements.txt first to leverage Docker cache
COPY ./requirements.txt /app/requirements.txt

WORKDIR /app

RUN pip install -r requirements.txt

COPY . /app

ENTRYPOINT [ "python" ]

CMD [ "app.py" ]