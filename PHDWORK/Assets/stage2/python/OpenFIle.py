import collada.animation as anim
import collada
FileLocation = 'D:\Walker\PHDWORK\Assets\stage2\AnimationFiles\Cube.DAE'

col = collada.Collada(FileLocation, ignore=[collada.DaeUnsupportedError,
                                            collada.DaeBrokenRefError])
#testing = anim.Animation.load(FileLocation,
print("test")