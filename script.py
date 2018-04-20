import numpy as np

pt_num = 31941
with open('new_3dmm.obj') as f:
    content = f.readlines()
content = [x.strip() for x in content]
pt = np.zeros((pt_num,3))
for i in range(1,pt_num+1):
    tmp = content[i].split(' ')
    pt[i-1,:]= np.array( [tmp[1],tmp[2],tmp[3]])

unity_pt = np.loadtxt('Unity_pt.txt',delimiter =',')

index= np.zeros((len(unity_pt),1))
for i in range(len(unity_pt)):
    match = unity_pt[i]
    for j in range(len(pt)):
        if(pt[j,0]==-match[0] and pt[j,1]==match[1]  and pt[j,2]==match[2]  ):
            index[i,0]=j
            break
np.savetxt('Assets/script/unity_index.txt',index,fmt='%d')