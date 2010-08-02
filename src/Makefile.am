ASSEMBLY = gir2gapi.exe
TARGETS = $(ASSEMBLY) $(ASSEMBLY).mdb
pkglib_DATA = $(TARGETS)
bin_SCRIPTS = gir2gapi

sources =				\
	Callback.cs			\
	Class.cs			\
	ClassStruct.cs			\
	Constant.cs			\
	Converter.cs			\
	Ctor.cs				\
	Enumeration.cs			\
	Field.cs			\
	GapiDocument.cs			\
	GirDocument.cs			\
	Interface.cs			\
	Mangler.cs			\
	Method.cs			\
	Namespace.cs			\
	Parameter.cs			\
	Parameters.cs			\
	Property.cs			\
	Record.cs			\
	ReturnValue.cs			\
	Signal.cs			\
	SymbolTable.cs			\
	Type.cs				\
	VirtualMethod.cs		\
	#

resources =				\
	empty-api.xml

DISTCLEANFILES = gir2gapi

EXTRA_DIST = $(sources) $(resources) gir2gapi.in

CLEANFILES = $(TARGETS)

build_sources = $(addprefix $(srcdir)/, $(sources))
build_resources = $(addprefix $(srcdir)/, $(resources))

$(TARGETS): $(build_sources) $(build_resources)
	$(CSC) -debug -out:$(ASSEMBLY) $(addprefix -resource:, $(build_resources)) $(build_sources)

