
def unit(v) : return (v,[])
def bind(mv,mf) :
    if(mv.value):
        return ( mf(mv.Value) ,  )


def addOne(x):
    val = x+1
    logmsg = "x+1 = %s" %val
    return (val,[logmsg])














