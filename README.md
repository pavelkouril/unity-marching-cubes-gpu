# Marching Cubes GPU implementation for Unity

Developed with Unity 5.6, might work on older versions as well. No guarantee tho. Requires DX11 hardware. Not honestly sure about mobile phones, but my guess is it won't work. Feel free to correct me.

The generating of density field is done on CPU, but for demonstration purposes it should be enough. You will probably want to modify the `MarchingCubes.DensityTexture` from `Texture3D` to `RenderTexture` in a real application.

`ProceduralGeometry` shader used for drawing the MC is barebone and contains only diffuse lightning; replace it with a custom one, modified to your liking. :)

Any pull requests and issues are welcome.

## Credits

Implementing this would not be possible without:

- The algorithm and lookup tables by Paul Bourke <http://paulbourke.net/geometry/polygonise/>
- The gradient normals calculation <http://www.angelfire.com/linux/myp/MCAdvanced/MCImproved.html>
- Vilém Otte <https://github.com/Zgragselus>, and his debugging/troubleshooting skills
