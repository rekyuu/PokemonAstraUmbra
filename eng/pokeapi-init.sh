#! /usr/bin/env bash

git submodule update --init --recursive
cd "external/pokeapi"

python -m venv .venv
.venv/bin/pip install -r requirements.txt
.venv/bin/python manage.py migrate --settings=config.local
echo "from data.v2.build import build_all; build_all()" | .venv/bin/python manage.py shell --settings=config.local