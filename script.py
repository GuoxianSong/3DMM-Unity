import numpy as np

def Match():
    pt_num = 31941
    with open('Assets/face.obj') as f:
        content = f.readlines()
    content = [x.strip() for x in content]
    pt = np.zeros((pt_num,3))
    for i in range(0,pt_num):
        tmp = content[i].split(' ')
        pt[i,:]= np.array( [tmp[1],tmp[2],tmp[3]])
    #pt = np.around(pt,1)
    unity_pt = np.loadtxt('Unity_pt.txt',delimiter =',')

    index= np.zeros((len(unity_pt),1))
    for i in range(len(unity_pt)):
        match = unity_pt[i]
        for j in range(len(pt)):
            if(pt[j,0]==-match[0] and pt[j,1]==match[1]  and pt[j,2]==match[2]  ):
                index[i,0]=j
                break
    np.savetxt('Assets/script/unity_index.txt',index,fmt='%d')

def Reverse():
    with open('Assets/new_3dmm.obj') as f:
        content = f.readlines()
    content = [x.strip() for x in content]


    with open("Assets/face.obj", "w") as text_file:
        for i in range(len(content)):
            tmp = content[i].split(' ')
            if (tmp[0] == "v"):
                text_file.write(content[i])
            else:
                line_ = tmp[0]+" "+tmp[1]+" "+tmp[3]+" "+tmp[2]
                text_file.write(line_)
            text_file.write('\n')
        text_file.closed;


def UDP_send():
    import socket
    str_=""
    for i in range(79):
        str_+='0,'
    bytesToSend = str.encode(str_[:-1])
    serverAddressPort = ("127.0.0.1", 8051)
    UDPClientSocket = socket.socket(family=socket.AF_INET, type=socket.SOCK_DGRAM)
    UDPClientSocket.sendto(bytesToSend, serverAddressPort)


UDP_send()