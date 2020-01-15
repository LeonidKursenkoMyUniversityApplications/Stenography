# Stenography
A program that hides a text message in an image.

The resulting program splits the incoming text message into bytes (each character takes 2 bytes), bytes into bits, 
and sequentially writes the received bits of the incoming text message into the lower bit of each channel (R, G, B) of each pixel. 

In order for the program to identify the end of the message, 16 zero bits are added at the end. 
To receive a message from a picture, the program sequentially reads the lowest bit of each channel in each pixel 
until 16 consecutive null bits indicate the end of the message (the program considers that the number of bits read must be 
a multiple of 16). The read bits are grouped into bytes, a text message is received from the bytes. 
The approach used allows you to hide 

`messages length = picture width * picture height * 3/16 - 1`.

This is one of my practical work on the subject of information security. The program was created during my fourth-year studies in 2017.
